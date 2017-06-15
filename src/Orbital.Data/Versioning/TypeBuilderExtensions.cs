using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Orbital.Data.Versioning
{
    internal static class TypeBuilderExtensions
    {
        public static TypeDefinition DefineType(this ModuleDefinition module, string name, TypeAttributes attr, TypeReference baseType)
        {
            var type = new TypeDefinition(module.Name, name, attr, baseType);
            module.Types.Add(type);
            return type;
        }

        public static MethodDefinition DefineMethod(this TypeDefinition type, string name, MethodAttributes attr, TypeReference returnType)
        {
            var method = new MethodDefinition(name, attr, returnType);
            type.Methods.Add(method);
            return method;
        }

        public static PropertyDefinition DefineProperty(this TypeDefinition type, string name, PropertyAttributes attr, TypeReference propertyType)
        {
            var property = new PropertyDefinition(name, attr, propertyType);
            type.Properties.Add(property);
            return property;
        }

        public static FieldDefinition DefineField(this TypeDefinition type, string name, FieldAttributes attr, TypeReference fieldType)
        {
            var field = new FieldDefinition(name, attr, fieldType);
            type.Fields.Add(field);
            return field;
        }
        
        public static PropertyDefinition DefineAutoProperty(this TypeDefinition type, string name, TypeReference propertyType)
        {
            // backing field
            var field = type.DefineField("__" + name, FieldAttributes.Private, propertyType);

            const MethodAttributes propertyAttr = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig | MethodAttributes.Virtual;

            // getter
            var getBuilder = type.DefineMethod("get_" + name, propertyAttr, propertyType);

            var getIL = getBuilder.Body.GetILProcessor();
            getIL.Emit(OpCodes.Ldarg_0);
            getIL.Emit(OpCodes.Ldfld, field);
            getIL.Emit(OpCodes.Ret);

            // setter
            var setBuilder = type.DefineMethod("set_" + name, propertyAttr, type.Module.TypeSystem.Void);
            setBuilder.Parameters.Add(new ParameterDefinition(propertyType));

            var setIL = setBuilder.Body.GetILProcessor();
            setIL.Emit(OpCodes.Ldarg_0);
            setIL.Emit(OpCodes.Ldarg_1);
            setIL.Emit(OpCodes.Stfld, field);
            setIL.Emit(OpCodes.Ret);
            
            // property
            var property = type.DefineProperty(name, PropertyAttributes.None, propertyType);

            property.GetMethod = getBuilder;
            property.SetMethod = setBuilder;
            
            return property;
        }
    }
}