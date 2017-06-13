using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Orbital.Data.Versioning
{
    internal static class TypeBuilderExtensions
    {
        public static PropertyBuilder DefineAutoProperty(this TypeBuilder typeBuilder, string name, Type type)
        {
            var fieldBuilder = typeBuilder.DefineField("__" + name, type, FieldAttributes.Private);

            const MethodAttributes propertyAttr = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig | MethodAttributes.Virtual;

            var propertyGetBuilder = typeBuilder.DefineMethod("get_" + name, propertyAttr, type, Type.EmptyTypes);
            var propertyGetIl = propertyGetBuilder.GetILGenerator();
            propertyGetIl.Emit(OpCodes.Ldarg_0);
            propertyGetIl.Emit(OpCodes.Ldfld, fieldBuilder);
            propertyGetIl.Emit(OpCodes.Ret);

            var propertSetBuilder = typeBuilder.DefineMethod("set_" + name, propertyAttr, null, new[] { type });
            var propertySetIl = propertSetBuilder.GetILGenerator();

            propertySetIl.Emit(OpCodes.Ldarg_0);
            propertySetIl.Emit(OpCodes.Ldarg_1);
            propertySetIl.Emit(OpCodes.Stfld, fieldBuilder);
            propertySetIl.Emit(OpCodes.Ret);

            var propertyBuilder = typeBuilder.DefineProperty(name, PropertyAttributes.None, type, Type.EmptyTypes);

            propertyBuilder.SetGetMethod(propertyGetBuilder);
            propertyBuilder.SetSetMethod(propertSetBuilder);

            return propertyBuilder;
        }
    }
}