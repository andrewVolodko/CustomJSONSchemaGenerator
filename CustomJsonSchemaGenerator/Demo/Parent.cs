using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CustomJsonSchemaGenerator.Generator.CustomAttributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;

namespace CustomJsonSchemaGenerator.Demo
{
    public class Parent
    {
        public string FirstName { get; set; }

        public int Age { get; set; }

        public double Height { get; set; }

        public Child FavChild;

        public Child[] OtherChildren;

        public bool IsCalm { get; set; }
    }

    public class Child
    {
        public string Name { get; set; }

        public int ChildAge { get; set; }

        public string ChildUUID;
    }
}