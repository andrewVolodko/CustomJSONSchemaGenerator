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
        internal static Type BuildTypeWithTypesToGenerateJsonSchemaByAttribute()
        {
            var methods = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assemble => assemble.GetTypes())
                .SelectMany(type => type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic |
                                                    BindingFlags.Instance | BindingFlags.Static))
                .Where(method => method.GetMethodBody() != null);

            var typesToGenerateSchemas = new Dictionary<string, Type>();

            foreach (var method in methods)
            {
                var instructions = method.GetInstructions();

                for (var i = 2; i < instructions.Count; i++)
                {
                    dynamic operand = instructions[i].Operand;
                    if (instructions[i].OpCode.OperandType.Equals(OperandType.InlineMethod) && operand.Name.Equals("GetJsonSchema"))
                    {
                        if (instructions[i - 2].OpCode.OperandType.Equals(OperandType.InlineTok))
                        {
                            operand = instructions[i - 2].Operand;
                            var type = Type.GetType(operand.AssemblyQualifiedName);

                            var fullName = type.FullName;
                            if (!typesToGenerateSchemas.ContainsKey(fullName!))
                                typesToGenerateSchemas.Add(fullName, type);
                        }
                    }
                }
            }

            var classBuilder = new DynamicClassBuilder("TypesToCreateJSchema");
            return classBuilder.CreateType(typesToGenerateSchemas.Keys.ToArray(), typesToGenerateSchemas.Values.ToArray());
        }
    }
}