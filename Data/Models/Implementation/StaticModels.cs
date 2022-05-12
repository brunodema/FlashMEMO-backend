using Data.Repository.Interfaces;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models.Implementation
{
    public class StaticModels
    {
        public class Language : IDatabaseItem<string>
        {
            [Key]
            public string ISOCode { get; set; } = "";
            public string Name { get; set; } = "";

            [JsonIgnore]
            [NotMapped]
            public string DbId { get => ISOCode; set => ISOCode = value; }
        }
    }
}
