using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using CustomJsonSchemaGenerator.Tests.Tests;
using Mono.Reflection;

namespace CustomJsonSchemaGenerator.Generator.Helpers
{
    internal static class CustomTypeBuilder
    {
        /// <summary>
        /// Obtaining all types which require JSON schemas, and generating one big common type
        /// with properties of types obtained.
        /// If obtained type is generic type, Generic Parameter Constraint HAVE to be specified.
        /// Algorithm searches for all types that inherit or implement the constraint and makes generic type
        /// E.g. there is Response<T> where T: IResponse
        /// MeetingObj implements IResponse
        /// So that algorithm will generate Response<MeetingObj> generic type, in order to generate such JSON schema in the future
        /// </summary>
        /// <returns>Common type with props of types for which JSON schemas required</returns>
        internal static Type BuildTypeWithTypesToGenerateJsonSchemaByAttribute(Type attributeType, string customTypeName)
        {

            MethodBase methodBase = typeof(JSchemaGenerationUnitTests).GetMethod("AssertClassSchemas");
            var instructions = methodBase.GetInstructions();

            var typesToGenerateSchema = new List<Type>();

            for (var i = 2; i < instructions.Count; i++)
            {
                dynamic operand = instructions[i].Operand;
                if (instructions[i].OpCode.OperandType.Equals(OperandType.InlineMethod) && operand.Name.Equals("GetJsonSchema"))
                {
                    if (instructions[i - 2].OpCode.OperandType.Equals(OperandType.InlineTok))
                    {
                        dynamic operandTest = instructions[i - 2].Operand;
                        var testType = Type.GetType(operandTest.AssemblyQualifiedName);

                        typesToGenerateSchema.Add(testType);
                    }
                }
            }



            var types = new Dictionary<string, Type>();

            var assembleTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assemble => assemble.GetTypes()).ToList();

            var typesToGenerateJsonSchema = assembleTypes
                .Where(type => Attribute.IsDefined(type, attributeType))
                .ToList();

            foreach (var type in typesToGenerateJsonSchema)
            {
                string fullName;
                // If current type is generic, it's necessary to obtain all it's constraints
                // to understand which type could be used with this generic, and so that generate schema for it
                if (type.IsGenericType)
                {
                    var constraintType = type.GetGenericArguments()[0].GetGenericParameterConstraints()[0];
                    var typesImplOrInheritConstraint = assembleTypes
                        .Where(t => constraintType.IsAssignableFrom(t) && t.FullName != constraintType.FullName && !t.IsAbstract)
                        .ToList();

                    foreach (var constructedGenericType in typesImplOrInheritConstraint.Select(
                        typeImplOrInheritConstraint => type.MakeGenericType(typeImplOrInheritConstraint)))
                    {
                        fullName = constructedGenericType.FullName;
                        if (!types.ContainsKey(fullName!))
                            types.Add(fullName, constructedGenericType);
                    }
                }
                else
                {
                    fullName = type.FullName;
                    if (!types.ContainsKey(fullName!))
                        types.Add(fullName, type);
                }
            }

            var classBuilder = new DynamicClassBuilder(customTypeName);
            return classBuilder.CreateType(types.Keys.ToArray(), types.Values.ToArray());
        }
    }
}