using CustomJSONGenerator.Generator;
using Newtonsoft.Json;

namespace CustomJSONGenerator.Tests.Models
{
    public class SimpleInteger
    {
        [GenerateJsonSchema]
        public class SimpleIntegerWithJsonPropertyName
        {
            [JsonProperty("simpleIntegerName")]
            public int SimpleInteger;
        }

        [GenerateJsonSchema]
        public class SimpleIntegerWithRequiredAlways
        {
            [JsonProperty(Required = Required.Always)]
            public int SimpleInteger;
        }

        [GenerateJsonSchema]
        public class SimpleIntegerWithRequiredDefault
        {
            [JsonProperty(Required = Required.Default)]
            public int SimpleInteger;
        }

        [GenerateJsonSchema]
        public class SimpleIntegersWithMinimum
        {
            [Minimum(byte.MinValue)]
            public byte SimpleIntegerByte;
            [Minimum(sbyte.MinValue)]
            public sbyte SimpleIntegerSByte;
            [Minimum(short.MinValue)]
            public short SimpleIntegerShort;
            [Minimum(ushort.MinValue)]
            public ushort SimpleIntegerUShort;
            [Minimum(int.MinValue)]
            public int SimpleIntegerInt;
            [Minimum(uint.MinValue)]
            public uint SimpleIntegerUInt;
            [Minimum(long.MinValue)]
            public long SimpleIntegerLong;
            [Minimum(ulong.MinValue)]
            public ulong SimpleIntegerULong;
        }

        [GenerateJsonSchema]
        public class SimpleIntegersWithExclusiveMinimum
        {
            [Minimum(byte.MinValue), ExclusiveMinimum]
            public byte SimpleIntegerByte;
            [Minimum(sbyte.MinValue), ExclusiveMinimum]
            public sbyte SimpleIntegerSByte;
            [Minimum(short.MinValue), ExclusiveMinimum]
            public short SimpleIntegerShort;
            [Minimum(ushort.MinValue), ExclusiveMinimum]
            public ushort SimpleIntegerUShort;
            [Minimum(int.MinValue), ExclusiveMinimum]
            public int SimpleIntegerInt;
            [Minimum(uint.MinValue), ExclusiveMinimum]
            public uint SimpleIntegerUInt;
            [Minimum(long.MinValue), ExclusiveMinimum]
            public long SimpleIntegerLong;
            [Minimum(ulong.MinValue), ExclusiveMinimum]
            public ulong SimpleIntegerULong;
        }

        [GenerateJsonSchema]
        public class SimpleIntegersWithMaximum
        {
            [Maximum(byte.MaxValue)]
            public byte SimpleIntegerByte;
            [Maximum(sbyte.MaxValue)]
            public sbyte SimpleIntegerSByte;
            [Maximum(short.MaxValue)]
            public short SimpleIntegerShort;
            [Maximum(ushort.MaxValue)]
            public ushort SimpleIntegerUShort;
            [Maximum(int.MaxValue)]
            public int SimpleIntegerInt;
            [Maximum(uint.MaxValue)]
            public uint SimpleIntegerUInt;
            [Maximum(long.MaxValue)]
            public long SimpleIntegerLong;
            [Maximum(ulong.MaxValue)]
            public ulong SimpleIntegerULong;
        }

        [GenerateJsonSchema]
        public class SimpleIntegersWithExclusiveMaximum
        {
            [Maximum(byte.MaxValue), ExclusiveMaximum]
            public byte SimpleIntegerByte;
            [Maximum(sbyte.MaxValue), ExclusiveMaximum]
            public sbyte SimpleIntegerSByte;
            [Maximum(short.MaxValue), ExclusiveMaximum]
            public short SimpleIntegerShort;
            [Maximum(ushort.MaxValue), ExclusiveMaximum]
            public ushort SimpleIntegerUShort;
            [Maximum(int.MaxValue), ExclusiveMaximum]
            public int SimpleIntegerInt;
            [Maximum(uint.MaxValue), ExclusiveMaximum]
            public uint SimpleIntegerUInt;
            [Maximum(long.MaxValue), ExclusiveMaximum]
            public long SimpleIntegerLong;
            [Maximum(ulong.MaxValue), ExclusiveMaximum]
            public ulong SimpleIntegerULong;
        }

        [GenerateJsonSchema]
        public class SimpleIntegerWithMultipleOf
        {
            [MultipleOf(10)]
            public int SimpleInteger;
        }
    }
}