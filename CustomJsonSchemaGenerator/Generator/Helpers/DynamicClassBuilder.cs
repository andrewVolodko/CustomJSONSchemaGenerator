using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace CustomJsonSchemaGenerator.Generator.Helpers
{
    internal class DynamicClassBuilder
    {
        private readonly AssemblyName _assemblyName;
        public DynamicClassBuilder(string className) => _assemblyName = new AssemblyName(className);

        internal Type CreateType(List<(string typeId, Type type, Dictionary<Type, dynamic> attributesWithValues)> typesToGenerateSchemas)
        {
            var dynamicClass = CreateClass();
            CreateConstructor(dynamicClass);

            for (var i = 0; i < typesToGenerateSchemas.Count; i++)
            {
                CreateProperty(
                    dynamicClass,
                    typesToGenerateSchemas[i].typeId,
                    typesToGenerateSchemas[i].type,
                    typesToGenerateSchemas[i].attributesWithValues);
            }

            var type = dynamicClass.CreateType();

            return type;
        }

        private TypeBuilder CreateClass()
        {
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(_assemblyName, AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
            var typeBuilder = moduleBuilder.DefineType(_assemblyName.FullName
                                , TypeAttributes.Public |
                                TypeAttributes.Class |
                                TypeAttributes.AutoClass |
                                TypeAttributes.AnsiClass |
                                TypeAttributes.BeforeFieldInit |
                                TypeAttributes.AutoLayout
                                , null);
            return typeBuilder;
        }
        private static void CreateConstructor(TypeBuilder typeBuilder) =>
            typeBuilder.DefineDefaultConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);

        private static void CreateProperty(TypeBuilder typeBuilder, string propertyName, Type propertyType, Dictionary<Type, dynamic> attributesWithValues)
        {
            var fieldBuilder = typeBuilder.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);

            var propertyBuilder = typeBuilder.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);
            var getPropMthdBldr = typeBuilder.DefineMethod("get_" + propertyName, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, propertyType, Type.EmptyTypes);
            var getIl = getPropMthdBldr.GetILGenerator();

            getIl.Emit(OpCodes.Ldarg_0);
            getIl.Emit(OpCodes.Ldfld, fieldBuilder);
            getIl.Emit(OpCodes.Ret);

            var setPropMthdBldr = typeBuilder.DefineMethod("set_" + propertyName,
                  MethodAttributes.Public |
                  MethodAttributes.SpecialName |
                  MethodAttributes.HideBySig,
                  null, new[] { propertyType });

            var setIl = setPropMthdBldr.GetILGenerator();
            var modifyProperty = setIl.DefineLabel();
            var exitSet = setIl.DefineLabel();

            setIl.MarkLabel(modifyProperty);
            setIl.Emit(OpCodes.Ldarg_0);
            setIl.Emit(OpCodes.Ldarg_1);
            setIl.Emit(OpCodes.Stfld, fieldBuilder);

            setIl.Emit(OpCodes.Nop);
            setIl.MarkLabel(exitSet);
            setIl.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getPropMthdBldr);
            propertyBuilder.SetSetMethod(setPropMthdBldr);

            foreach (var (attrType, attrValue) in attributesWithValues)
            {
                var constructorInfo = attrType.GetConstructor(new Type[] { attrValue.GetType() });
                var attrBuilder = new CustomAttributeBuilder(constructorInfo, new [] { attrValue });

                propertyBuilder.SetCustomAttribute(attrBuilder);
            }
        }
    }
}
