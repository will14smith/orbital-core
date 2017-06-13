using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Orbital.Data.Versioning
{
    internal class VersionModelCustomizer : ModelCustomizer
    {
        public static readonly string ModelMappingAnnotation = "VersionModelMapping";

        public override void Customize(ModelBuilder modelBuilder, DbContext dbContext)
        {
            base.Customize(modelBuilder, dbContext);

            var modelMapping = new Dictionary<string, VersionEntityMapping>();

            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("Autogen.Versioning"), AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule("Autogen.Versioning");

            var entityTypes = modelBuilder.Model.GetEntityTypes().ToList();
            foreach (var entityType in entityTypes)
            {
                var historyModelType = CreateVersionEntity(modelBuilder, entityType, moduleBuilder);
                modelMapping.Add(entityType.ClrType.Name, historyModelType);
            }

            modelBuilder.Model.AddAnnotation(ModelMappingAnnotation, modelMapping);
        }

        private static VersionEntityMapping CreateVersionEntity(ModelBuilder modelBuilder, IMutableEntityType entityType, ModuleBuilder moduleBuilder)
        {
            var entityClrType = entityType.ClrType;
            var entityClrName = entityClrType.Name;
            
            var typeBuilder = moduleBuilder.DefineType(entityClrName + "Version");

            // Implement IVersionEntity<TEntity>
            typeBuilder.AddInterfaceImplementation(typeof(IVersionEntity<>).MakeGenericType(entityClrType));

            // Id & Date columns
            DefineColumnProperty(typeBuilder, nameof(IVersionEntity<object>.Id), typeof(long));
            var dateColumn = DefineColumnProperty(typeBuilder, nameof(IVersionEntity<object>.Date), typeof(DateTime));

            // Copy all the non-navigation columns from the entity, prefix the with Field_ to avoid collisions with version columns
            var fieldMappings = new Dictionary<IProperty, PropertyInfo>();
            foreach (var prop in entityType.GetProperties())
            {
                var property = DefineColumnProperty(typeBuilder, "Field_" + prop.Name, prop.ClrType);
                fieldMappings.Add(prop, property);
            }

            // Define the default & entity constructors
            DefineConstructors(typeBuilder, entityClrType, dateColumn, fieldMappings);
            // Finish implementing IVersionEntity<TEntity>
            DefineToEntity(typeBuilder, entityClrType, fieldMappings);

            // Register the new entity with EF
            var versionEntityType = typeBuilder.CreateTypeInfo().AsType();
            modelBuilder.Entity(versionEntityType, entityBuilder =>
            {
                var originalTableName = entityType.FindAnnotation("Relational:TableName")?.Value ?? entityClrName;
                entityBuilder.HasAnnotation("Relational:TableName", originalTableName + "_history");
            });
            
            return new VersionEntityMapping(entityClrType, versionEntityType, fieldMappings);
        }

        private static void DefineConstructors(TypeBuilder typeBuilder, Type entityType, PropertyBuilder dateColumn, Dictionary<IProperty, PropertyInfo> fieldMappings)
        {
            // default constructor
            typeBuilder.DefineDefaultConstructor(MethodAttributes.Public);

            // from entity constructor
            var constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new[] { entityType });

            var il = constructorBuilder.GetILGenerator();

            // base()
            il.Emit(OpCodes.Ldarg_0); // this
            il.Emit(OpCodes.Call, typeof(object).GetTypeInfo().GetConstructor(Type.EmptyTypes));

            // var entity = param as entityType;
            var entity = il.DeclareLocal(entityType);

            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Isinst, entityType);
            il.Emit(OpCodes.Stloc, entity);

            // if entity == null {
            var successLabel = il.DefineLabel();

            il.Emit(OpCodes.Ldloc, entity);
            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Ceq);
            il.Emit(OpCodes.Brfalse, successLabel);

            //     throw new InvalidOperationException
            il.Emit(OpCodes.Ldstr, "Unable to cast object to " + entityType.Name);
            il.Emit(OpCodes.Newobj, typeof(InvalidOperationException).GetTypeInfo().GetConstructor(new[] { typeof(string) }));
            il.Emit(OpCodes.Throw);

            // }
            il.MarkLabel(successLabel);

            // this.Date = DateTime.UtcNow;
            il.Emit(OpCodes.Ldarg_0); // this
            il.EmitCall(OpCodes.Call, typeof(DateTime).GetRuntimeProperty(nameof(DateTime.UtcNow)).GetMethod, Type.EmptyTypes);
            il.EmitCall(OpCodes.Call, dateColumn.SetMethod, Type.EmptyTypes);

            foreach (var fieldMapping in fieldMappings)
            {
                // this.A = entity.A;
                var entityField = fieldMapping.Key.PropertyInfo;
                var historyField = fieldMapping.Value;

                il.Emit(OpCodes.Ldarg_0); // this
                il.Emit(OpCodes.Ldloc, entity);
                il.EmitCall(OpCodes.Callvirt, entityField.GetMethod, Type.EmptyTypes);
                il.EmitCall(OpCodes.Call, historyField.SetMethod, Type.EmptyTypes);
            }

            // return
            il.Emit(OpCodes.Ret);
        }

        private static void DefineToEntity(TypeBuilder typeBuilder, Type entityType, Dictionary<IProperty, PropertyInfo> fieldMappings)
        {
            var methodBuilder = typeBuilder.DefineMethod(
                nameof(IVersionEntity<object>.ToEntity),
                MethodAttributes.Public | MethodAttributes.Virtual,
                entityType,
                Type.EmptyTypes);
            var il = methodBuilder.GetILGenerator();

            // var entity = new Entity();
            var entity = il.DeclareLocal(entityType);

            il.Emit(OpCodes.Newobj, entityType.GetTypeInfo().GetConstructor(Type.EmptyTypes));
            il.Emit(OpCodes.Stloc, entity);

            foreach (var fieldMapping in fieldMappings)
            {
                // entity.A = A;
                var entityField = fieldMapping.Key.PropertyInfo;
                var historyField = fieldMapping.Value;

                il.Emit(OpCodes.Ldloc, entity);
                il.Emit(OpCodes.Ldarg_0); // this
                il.EmitCall(OpCodes.Call, historyField.GetMethod, Type.EmptyTypes);
                il.EmitCall(OpCodes.Callvirt, entityField.SetMethod, Type.EmptyTypes);
            }

            // return entity
            il.Emit(OpCodes.Ldloc, entity);
            il.Emit(OpCodes.Ret);

            typeBuilder.DefineMethodOverride(
                methodBuilder,
                typeof(IVersionEntity<>).MakeGenericType(entityType).GetRuntimeMethod(nameof(IVersionEntity<object>.ToEntity), Type.EmptyTypes));
        }

        private static PropertyBuilder DefineColumnProperty(TypeBuilder typeBuilder, string id, Type type)
        {
            var columnAttributeConstructor = typeof(ColumnAttribute).GetTypeInfo().GetConstructor(new[] { typeof(string) });
            var columnAttributeBuilder = new CustomAttributeBuilder(columnAttributeConstructor, new object[] { id });

            var prop = typeBuilder.DefineAutoProperty(id, type);
            prop.SetCustomAttribute(columnAttributeBuilder);

            return prop;
        }
    }
}