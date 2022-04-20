using Data.Repository.Interfaces;
using System;

namespace Data.Models.Implementation
{
    public class StaticModels
    {
        public class Language : IDatabaseItem<Guid>
        {
            public Guid LanguageId { get; set; } = new Guid();
            public Guid DbId { get => LanguageId; set => LanguageId = value; }

            public string Name { get; set; } = "";
            public string ISOCode { get; set; } = "";
        }
    }
}
