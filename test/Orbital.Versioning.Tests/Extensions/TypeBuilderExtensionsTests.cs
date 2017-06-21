using System;
using System.Reflection;
using Mono.Cecil;
using Xunit;
using FieldAttributes = Mono.Cecil.FieldAttributes;
using MethodAttributes = Mono.Cecil.MethodAttributes;
using PropertyAttributes = Mono.Cecil.PropertyAttributes;
using TypeAttributes = Mono.Cecil.TypeAttributes;

namespace Orbital.Versioning.Tests.Extensions
{
    public class TypeBuilderExtensionsTests
    {
        private readonly AssemblyDefinition _assembly;
        private readonly ModuleDefinition _module;

        public TypeBuilderExtensionsTests()
        {
            var name = "Test_" + Guid.NewGuid().ToString("N");
            _assembly = AssemblyDefinition.CreateAssembly(new AssemblyNameDefinition(name, new Version(1, 0)), name, ModuleKind.Dll);
            _module = _assembly.MainModule;
        }

        [Fact]
        public void DefineType_SetsAllProperties()
        {
            var name = "TypeName";
            var attr = TypeAttributes.Public | TypeAttributes.Abstract;
            var baseType = _module.TypeSystem.Object;

            var type = _module.DefineType(name, attr, baseType);

            Assert.Equal(name, type.Name);
            Assert.Equal(attr, type.Attributes);
            Assert.Equal(baseType, type.BaseType);
        }
        [Fact]
        public void DefineType_AddsTypeToTheModule()
        {
            var type = _module.DefineType("TypeName", TypeAttributes.Public | TypeAttributes.Abstract, _module.TypeSystem.Object);

            Assert.Contains(type, _module.Types);
        }

        [Fact]
        public void DefineMethod_SetsAllProperties()
        {
            var type = new TypeDefinition("", "", TypeAttributes.NotPublic);

            var name = "MethodName";
            var attr = MethodAttributes.Public | MethodAttributes.Abstract;
            var returnType = _module.TypeSystem.Int16;

            var method = type.DefineMethod(name, attr, returnType);

            Assert.Equal(name, method.Name);
            Assert.Equal(attr, method.Attributes);
            Assert.Equal(returnType, method.ReturnType);
        }
        [Fact]
        public void DefineMethod_AddsTypeToTheModule()
        {
            var type = new TypeDefinition("", "", TypeAttributes.NotPublic);

            var method = type.DefineMethod("MethodName", MethodAttributes.Public, _module.TypeSystem.Int16);

            Assert.Contains(method, type.Methods);
        }

        [Fact]
        public void DefineProperty_SetsAllProperties()
        {
            var type = new TypeDefinition("", "", TypeAttributes.NotPublic);

            var name = "PropertyName";
            var attr = PropertyAttributes.SpecialName | PropertyAttributes.HasDefault;
            var propertyType = _module.TypeSystem.Int16;

            var property = type.DefineProperty(name, attr, propertyType);

            Assert.Equal(name, property.Name);
            Assert.Equal(attr, property.Attributes);
            Assert.Equal(propertyType, property.PropertyType);
        }
        [Fact]
        public void DefineProperty_AddsTypeToTheModule()
        {
            var type = new TypeDefinition("", "", TypeAttributes.NotPublic);

            var property = type.DefineProperty("PropertyName", PropertyAttributes.None, _module.TypeSystem.Int16);

            Assert.Contains(property, type.Properties);
        }

        [Fact]
        public void DefineField_SetsAllProperties()
        {
            var type = new TypeDefinition("", "", TypeAttributes.NotPublic);

            var name = "FieldName";
            var attr = FieldAttributes.Public | FieldAttributes.CompilerControlled;
            var fieldType = _module.TypeSystem.Int16;

            var field = type.DefineField(name, attr, fieldType);

            Assert.Equal(name, field.Name);
            Assert.Equal(attr, field.Attributes);
            Assert.Equal(fieldType, field.FieldType);
        }
        [Fact]
        public void DefineField_AddsTypeToTheModule()
        {
            var type = new TypeDefinition("", "", TypeAttributes.NotPublic);

            var field = type.DefineField("FieldName", FieldAttributes.Public, _module.TypeSystem.Int16);

            Assert.Contains(field, type.Fields);
        }

        [Fact]
        public void Build_ReturnsAssemblyContainingDefinedItems()
        {
            var typeDefinition = _module.DefineType("TypeName", TypeAttributes.Public, _module.TypeSystem.Object);

            var assembly = _assembly.Build();
            Assert.NotNull(assembly);

            var type = assembly.GetType(typeDefinition.FullName);
            Assert.NotNull(type);
        }

        [Fact]
        public void DefineConstructor_NoParameters_SetsAllProperties()
        {
            var typeDefinition = _module.DefineType("TypeName", TypeAttributes.Public, _module.TypeSystem.Object);

            var constructor = typeDefinition.DefineConstructor();

            Assert.Equal(".ctor", constructor.Name);
            Assert.Equal(MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName, constructor.Attributes);
            Assert.Equal(_module.TypeSystem.Void, constructor.ReturnType);
            Assert.Empty(constructor.Parameters);
        }
        [Fact]
        public void DefineConstructor_Parameters_HasParameters()
        {
            var typeDefinition = _module.DefineType("TypeName", TypeAttributes.Public, _module.TypeSystem.Object);

            var param1 = new ParameterDefinition(_module.TypeSystem.Int32);
            var param2 = new ParameterDefinition(_module.TypeSystem.String);

            var constructor = typeDefinition.DefineConstructor(param1, param2);

            Assert.Equal(new[] { param1, param2 }, constructor.Parameters);
        }

        [Fact]
        public void DefineDefaultConstructor_CanActivateType()
        {
            var typeDefinition = _module.DefineType("TypeName", TypeAttributes.Public, _module.TypeSystem.Object);
            typeDefinition.DefineDefaultConstructor();

            var assembly = _assembly.Build();
            var type = assembly.GetType(typeDefinition.FullName);

            var instance = Activator.CreateInstance(type);
            Assert.NotNull(instance);
        }

        [Fact]
        public void DefineAutoProperty_PropertyReturnsDefaultValue()
        {
            var typeDefinition = _module.DefineType("TypeName", TypeAttributes.Public, _module.TypeSystem.Object);
            typeDefinition.DefineDefaultConstructor();

            var propertyDefinition = typeDefinition.DefineAutoProperty("Property", _module.TypeSystem.Int32);

            var assembly = _assembly.Build();
            var type = assembly.GetType(typeDefinition.FullName);
            var property = type.GetRuntimeProperty(propertyDefinition.Name);

            var instance = Activator.CreateInstance(type);

            Assert.Equal(default(int), property.GetValue(instance));
        }
        [Fact]
        public void DefineAutoProperty_PropertyReturnsValueAfterSet()
        {
            var typeDefinition = _module.DefineType("TypeName", TypeAttributes.Public, _module.TypeSystem.Object);
            typeDefinition.DefineDefaultConstructor();

            var propertyDefinition = typeDefinition.DefineAutoProperty("Property", _module.TypeSystem.Int32);

            var assembly = _assembly.Build();
            var type = assembly.GetType(typeDefinition.FullName);
            var property = type.GetRuntimeProperty(propertyDefinition.Name);

            var instance = Activator.CreateInstance(type);

            var value = 10;

            property.SetValue(instance, value);
            Assert.Equal(value, property.GetValue(instance));
        }
    }
}
