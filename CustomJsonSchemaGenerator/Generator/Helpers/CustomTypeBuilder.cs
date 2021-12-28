using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
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
        internal static Type BuildTypeWithTypesToGenerateJsonSchemaByAttribute()
        {
            var methods = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assemble => assemble.GetTypes())
                .SelectMany(type => type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic |
                                                    BindingFlags.Instance | BindingFlags.Static))
                .Where(method => method.GetMethodBody() != null);

            var typesToGenerateSchemas =
                new List<(string typeId, Type type, Dictionary<Type, dynamic> attributesWithValues)>();

            foreach (var method in methods)
            {
                var instructions = method.GetInstructions();

                for (var i = 2; i < instructions.Count; i++)
                {
                    dynamic operand = instructions[i].Operand;
                    if (instructions[i].OpCode.OperandType.Equals(OperandType.InlineMethod) &&
                        operand.Name.Equals("GetJsonSchema"))
                    {
                        int indexOfProvidedType = 0;
                        Type providedType = null;
                        for (var k = i; k > 2; k--)
                        {
                            if (instructions[k].OpCode.OperandType.Equals(OperandType.InlineTok))
                            {
                                indexOfProvidedType = k;

                                operand = instructions[k].Operand;
                                providedType = Type.GetType(operand.AssemblyQualifiedName);

                                break;
                            }
                        }

                        // Find provided attributes with their values
                        Dictionary<Type, dynamic> attributesWithValues = null;
                        for (var u = indexOfProvidedType + 6; u < i; u += 5)
                        {
                            attributesWithValues ??= new Dictionary<Type, dynamic>();

                            operand = instructions[u + 1].Operand;
                            var attributeType = Type.GetType(operand.DeclaringType.AssemblyQualifiedName);

                            if (operand.DeclaringType.DeclaredProperties[0].PropertyType.Name == "Boolean")
                            {
                                var value = instructions[u].OpCode.Value == 23;
                                attributesWithValues.Add(attributeType, value);
                            }
                            else
                            {
                                attributesWithValues.Add(attributeType, instructions[u].Operand);
                            }
                        }

                        var propertyId = GenerateIdForType(providedType, attributesWithValues);

                        var doesValueAlreadyExist = typesToGenerateSchemas.Any(el => el.typeId.Equals(propertyId));
                        if (!doesValueAlreadyExist)
                            typesToGenerateSchemas.Add((propertyId, providedType, attributesWithValues));
                    }
                }
            }

            var classBuilder = new DynamicClassBuilder("TypesToCreateJSchema");
            return classBuilder.CreateType(typesToGenerateSchemas);
        }

        private static string GenerateIdForType(Type type, Dictionary<Type, dynamic> attributesWithValues = null)
        {
            using SHA256 hash = SHA256.Create();

            var resultId = GetSha256Hash(type.FullName);

            if (attributesWithValues != null)
            {
                foreach (var (attrType, value) in attributesWithValues)
                {
                    resultId += GetSha256Hash(attrType.FullName) + value.GetHashCode().ToString();
                }
            }

            return $"{type.Name}_{resultId}";
        }

        private static string GetSha256Hash(string value)
        {
            using var hash = SHA256.Create();
            return string.Concat(hash
                .ComputeHash(Encoding.UTF8.GetBytes(value))
                .Select(item => item.ToString("x2")));
        }
    }
}