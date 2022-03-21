using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CustomJsonSchemaGenerator.Generator.Helpers
{
    public static class RandomHelper
    {
        public static string GenerateIdForType(Type type, Dictionary<Type, dynamic> attributesWithValues = null)
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

        public static string GetSha256Hash(string value)
        {
            using var hash = SHA256.Create();
            return string.Concat(hash
                .ComputeHash(Encoding.UTF8.GetBytes(value))
                .Select(item => item.ToString("x2")));
        }
    }
}