using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Orbital.Data.Versioning
{
    internal static class TypeBuilderExtensions
    {
        public static PropertyDefinition DefineAutoProperty(this TypeDefinition typeBuilder, string name, TypeReference type)
        {
            // backing field
            var field = new FieldDefinition("__" + name, FieldAttributes.Private, type);
            typeBuilder.Fields.Add(field);

            const MethodAttributes propertyAttr = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig | MethodAttributes.Virtual;

            // getter
            var getBuilder = new MethodDefinition("get_" + name, propertyAttr, type);
            typeBuilder.Methods.Add(getBuilder);

            var getIL = getBuilder.Body.GetILProcessor();
            getIL.Emit(OpCodes.Ldarg_0);
            getIL.Emit(OpCodes.Ldfld, field);
            getIL.Emit(OpCodes.Ret);

            // setter
            var setBuilder = new MethodDefinition("set_" + name, propertyAttr, typeBuilder.Module.TypeSystem.Void);
            setBuilder.Parameters.Add(new ParameterDefinition(type));
            typeBuilder.Methods.Add(setBuilder);

            var setIL = setBuilder.Body.GetILProcessor();
            setIL.Emit(OpCodes.Ldarg_0);
            setIL.Emit(OpCodes.Ldarg_1);
            setIL.Emit(OpCodes.Stfld, field);
            setIL.Emit(OpCodes.Ret);
            
            var property = new PropertyDefinition(name, PropertyAttributes.None, type);
            typeBuilder.Properties.Add(property);

            property.GetMethod = getBuilder;
            property.SetMethod = setBuilder;


            return property;
        }
    }
}