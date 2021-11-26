using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using CustomJSONGenerator.Generator;
using Newtonsoft.Json;

namespace Tests.Model.Meeting
{
    [AllowAdditionalProperties(false)]
    public class NameEmailObj : IComparable<NameEmailObj>
    {
        [JsonProperty("name", Required = Required.Always), MinimumLength(1)]
        public string Name { get; set; }
        
        [JsonProperty("email", Required = Required.Always), RegularExpression("^.+(@itechart-group\\.com)$")]
        public string Email { get; set; }

        [JsonConstructor]
        public NameEmailObj(string name, string email)
        {
            Name = name;
            Email = email;
        }
        
        public int CompareTo([DisallowNull] NameEmailObj other)
        {
            return string.Compare(Email, other.Email, StringComparison.Ordinal);
        }
    }
}