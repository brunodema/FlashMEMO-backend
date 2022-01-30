using Business.Services.Implementation;
using Business.Services.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Business.Tools.DictionaryAPI.Oxford
{
    public class OxfordAPIDTO : DictionaryAPIDTO<OxfordAPIResponseModel>
    {
        public string SearchText { get; set; }
        public string LanguageCode { get; set; }
        public List<DictionaryAPIResult> Results { get; set; }

        public override DictionaryAPIDTO<OxfordAPIResponseModel> CreateDTO(OxfordAPIResponseModel oxfordResponse)
        {
            var dto = new OxfordAPIDTO();

            dto.SearchText = oxfordResponse.Id;
            dto.LanguageCode = oxfordResponse.Results[0].Language; // shouldn't be different accross entries anyway
            dto.Results = new List<DictionaryAPIResult>();

            foreach (var result in oxfordResponse.Results)
            {
                foreach (var lexicalEntry in result.LexicalEntries)
                {
                    var dictAPIResult = new DictionaryAPIResult()
                    {
                        LexicalCategory = lexicalEntry.LexicalCategory.Text,
                        PhoneticSpelling = "",
                        PronunciationFile = "",
                        Definitions = new List<string>(),
                        Examples = new List<string>(),
                    };

                    foreach (var entry in lexicalEntry.Entries)
                    {
                        dictAPIResult.PronunciationFile = entry.Pronunciations?.Select(p => p.AudioFile)?.FirstOrDefault() ?? ""; // won't bother with additional pronunciations/spellings for now
                        dictAPIResult.PhoneticSpelling = entry.Pronunciations?.Select(p => p.PhoneticSpelling)?.FirstOrDefault() ?? ""; // won't bother with additional pronunciations/spellings for now

                        foreach (var sense in entry.Senses)
                        {
                            if (sense.Definitions is not null) dictAPIResult.Definitions.AddRange(sense.Definitions.ToList());
                            if (sense.Examples is not null) dictAPIResult.Examples.AddRange(sense.Examples.Select(e => e.Text).ToList());
                        }
                    }

                    dto.Results.Add(dictAPIResult);
                }
            }

            return dto;
        }
    }

    public class OxfordAPIResponseModel : IDictionaryAPIResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("metadata")]
        public Metadata Metadata { get; set; }

        [JsonProperty("results")]
        public List<Result> Results { get; set; }

    }
    // created with the help of https://json2csharp.com/
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Metadata
    {
        [JsonProperty("operation")]
        public string Operation { get; set; }

        [JsonProperty("provider")]
        public string Provider { get; set; }

        [JsonProperty("schema")]
        public string Schema { get; set; }
    }

    public class Pronunciation
    {
        [JsonProperty("audioFile")]
        public string AudioFile { get; set; }

        [JsonProperty("dialects")]
        public List<string> Dialects { get; set; }

        [JsonProperty("phoneticNotation")]
        public string PhoneticNotation { get; set; }

        [JsonProperty("phoneticSpelling")]
        public string PhoneticSpelling { get; set; }
    }

    public class DomainClass
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class Register
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class Note
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class Example
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("registers")]
        public List<Register> Registers { get; set; }

        [JsonProperty("notes")]
        public List<Note> Notes { get; set; }
    }

    public class SemanticClass
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class Domain
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class Subsens
    {
        [JsonProperty("definitions")]
        public List<string> Definitions { get; set; }

        [JsonProperty("domainClasses")]
        public List<DomainClass> DomainClasses { get; set; }

        [JsonProperty("examples")]
        public List<Example> Examples { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("semanticClasses")]
        public List<SemanticClass> SemanticClasses { get; set; }

        [JsonProperty("shortDefinitions")]
        public List<string> ShortDefinitions { get; set; }

        [JsonProperty("domains")]
        public List<Domain> Domains { get; set; }

        [JsonProperty("registers")]
        public List<Register> Registers { get; set; }

        [JsonProperty("notes")]
        public List<Note> Notes { get; set; }
    }

    public class Synonym
    {
        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class ThesaurusLink
    {
        [JsonProperty("entry_id")]
        public string EntryId { get; set; }

        [JsonProperty("sense_id")]
        public string SenseId { get; set; }
    }

    public class Region
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class Sens
    {
        [JsonProperty("definitions")]
        public List<string> Definitions { get; set; }

        [JsonProperty("domainClasses")]
        public List<DomainClass> DomainClasses { get; set; }

        [JsonProperty("examples")]
        public List<Example> Examples { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("semanticClasses")]
        public List<SemanticClass> SemanticClasses { get; set; }

        [JsonProperty("shortDefinitions")]
        public List<string> ShortDefinitions { get; set; }

        [JsonProperty("registers")]
        public List<Register> Registers { get; set; }

        [JsonProperty("subsenses")]
        public List<Subsens> Subsenses { get; set; }

        [JsonProperty("synonyms")]
        public List<Synonym> Synonyms { get; set; }

        [JsonProperty("thesaurusLinks")]
        public List<ThesaurusLink> ThesaurusLinks { get; set; }

        [JsonProperty("regions")]
        public List<Region> Regions { get; set; }
    }

    public class GrammaticalFeature
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class Entry
    {
        [JsonProperty("etymologies")]
        public List<string> Etymologies { get; set; }

        [JsonProperty("homographNumber")]
        public string HomographNumber { get; set; }

        [JsonProperty("pronunciations")]
        public List<Pronunciation> Pronunciations { get; set; }

        [JsonProperty("senses")]
        public List<Sens> Senses { get; set; }

        [JsonProperty("grammaticalFeatures")]
        public List<GrammaticalFeature> GrammaticalFeatures { get; set; }
    }

    public class LexicalCategory
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class Phras
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class LexicalEntry
    {
        [JsonProperty("entries")]
        public List<Entry> Entries { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("lexicalCategory")]
        public LexicalCategory LexicalCategory { get; set; }

        [JsonProperty("phrases")]
        public List<Phras> Phrases { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class Result
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("lexicalEntries")]
        public List<LexicalEntry> LexicalEntries { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("word")]
        public string Word { get; set; }
    }

    public class Root
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("metadata")]
        public Metadata Metadata { get; set; }

        [JsonProperty("results")]
        public List<Result> Results { get; set; }

        [JsonProperty("word")]
        public string Word { get; set; }
    }
}
