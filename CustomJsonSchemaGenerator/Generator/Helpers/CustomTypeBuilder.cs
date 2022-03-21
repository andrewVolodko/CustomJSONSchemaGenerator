using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
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
                    // If current instruction type is InlineMethod and name of operand (method) equals "GetJsonSchema"
                    // then it's necessary to process this instruction
                    if (instructions[i].OpCode.OperandType.Equals(OperandType.InlineMethod) &&
                        operand.Name.Equals("GetJsonSchema"))
                    {
                        var indexOfProvidedType = 0;
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

                        // Find provided attributes parameters with their values
                        Dictionary<Type, dynamic> attributesWithValues = null;

                        var startIndexIncreaser = 6;
                        var step = 5;
                        for (var u = indexOfProvidedType + startIndexIncreaser; u < i; u += step)
                        {
                            attributesWithValues ??= new Dictionary<Type, dynamic>();

                            // It's necessary to obtain type of attribute value
                            // Type of attribute stores by current index plus 1
                            operand = instructions[u + 1].Operand;
                            var attributeType = Type.GetType(operand.DeclaringType.AssemblyQualifiedName);

                            var opCode = instructions[u].OpCode;
                            dynamic value;
                            if (operand.DeclaringType.DeclaredProperties[0].PropertyType.Name == "Boolean")
                            {
                                value = opCode.Value == 23;
                            }
                            else
                            {
                                // If int attribute value is less than 9, Operand will be null
                                // So that it's necessary to obtain attribute value from OpCode name
                                value = instructions[u].Operand ??
                                            int.Parse(Regex.Match(opCode.Name!, "(?!.*\\..*\\.)\\d{1}$").Value);
                            }

                            attributesWithValues.Add(attributeType, value);
                        }

                        // It's necessary to generate unique Id for our type
                        // Since we can have several identical objects, but their attributes set is different
                        // So that for these objects we need unique JSON schema
                        var propertyId = RandomHelper.GenerateIdForType(providedType, attributesWithValues);

                        // If JSON schema was already generated for the same object,
                        // there is no need to generate the new one
                        var doesValueAlreadyExist = typesToGenerateSchemas.Any(el => el.typeId.Equals(propertyId));
                        if (!doesValueAlreadyExist)
                            typesToGenerateSchemas.Add((propertyId, providedType, attributesWithValues));
                    }
                }
            }

            var classBuilder = new DynamicClassBuilder("TypesToCreateJSchema");
            return classBuilder.CreateType(typesToGenerateSchemas);
        }
    }
}