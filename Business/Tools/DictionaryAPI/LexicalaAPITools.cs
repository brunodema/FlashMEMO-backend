using Business.Services.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Business.Tools.DictionaryAPI.Lexicala
{
    public class LexicalaAPIDTO : IDictionaryAPIDTO<LexicalaAPIResponseModel>
    {
        public string SearchText { get; set; }
        public string LanguageCode { get; set; }
        public List<DictionaryAPIResult> Results { get; set; }

        public override IDictionaryAPIDTO<LexicalaAPIResponseModel> CreateDTO(LexicalaAPIResponseModel lexicalaResponse)
        {
            var dto = new LexicalaAPIDTO();

            dto.LanguageCode = lexicalaResponse.Results[0].Language;  // shouldn't be different accross entries anyway
            dto.SearchText = lexicalaResponse.Results[0].Headword.Text;  // shouldn't be different accross entries anyway
            dto.Results = new List<DictionaryAPIResult>();

            foreach (var result in lexicalaResponse.Results)
            {
                var dictAPIResult = new DictionaryAPIResult()
                {
                    LexicalCategory = result.Headword.Pos,
                    PhoneticSpelling = result.Headword.Pronunciation?.Value ?? "",
                    PronunciationFile = "",
                    Definitions = new List<string>(),
                    Examples = new List<string>(),
                };

                foreach (var sense in result.Senses)
                {
                    if (sense.Definition is not null) dictAPIResult.Definitions.Add(sense.Definition);
                    if (sense.Examples is not null) dictAPIResult.Examples.AddRange(sense.Examples.Select(s => s.Text).ToList());
                }

                dto.Results.Add(dictAPIResult);
            }

            return dto;
        }
    }

    /// <summary>
    /// Represents the mapping of properties of the json object into a C# one (data extracted via https://json2csharp.com/).
    /// </summary>
    public class LexicalaAPIResponseModel : IDictionaryAPIResponse
    {
        [JsonProperty("n_results")]
        public int NResults { get; set; }

        [JsonProperty("page_number")]
        public int PageNumber { get; set; }

        [JsonProperty("results_per_page")]
        public int ResultsPerPage { get; set; }

        [JsonProperty("n_pages")]
        public int NPages { get; set; }

        [JsonProperty("available_n_pages")]
        public int AvailableNPages { get; set; }

        [JsonProperty("results")]
        public List<Result> Results { get; set; }
    }

    // created with the help of https://json2csharp.com/
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Pronunciation
    {
        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public class Headword
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("pronunciation")]
        public Pronunciation Pronunciation { get; set; }

        [JsonProperty("pos")]
        public string Pos { get; set; }

        [JsonProperty("homograph_number")]
        public int HomographNumber { get; set; }

        [JsonProperty("subcategorization")]
        public string Subcategorization { get; set; }
    }

    //public class AlternativeScripts
    //{
    //    [JsonProperty("romaji")]
    //    public string Romaji { get; set; }
    //}

    //public class Ja
    //{
    //    [JsonProperty("text")]
    //    public string Text { get; set; }

    //    [JsonProperty("alternative_scripts")]
    //    public AlternativeScripts AlternativeScripts { get; set; }
    //}

    //public class Sv
    //{
    //    [JsonProperty("text")]
    //    public string Text { get; set; }

    //    [JsonProperty("gender")]
    //    public string Gender { get; set; }
    //}

    public class Example
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("translations")]
        [JsonIgnore]
        public object Translations { get; set; }
    }

    public class CompositionalPhras
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("definition")]
        public string Definition { get; set; }

        [JsonProperty("translations")]
        [JsonIgnore]
        public object Translations { get; set; }

        [JsonProperty("examples")]
        public List<Example> Examples { get; set; }
    }

    public class Sens
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("definition")]
        public string Definition { get; set; }

        [JsonProperty("subcategorization")]
        public string Subcategorization { get; set; }

        [JsonProperty("translations")]
        [JsonIgnore]
        public object Translations { get; set; }

        [JsonProperty("examples")]
        public List<Example> Examples { get; set; }

        [JsonProperty("compositional_phrases")]
        public List<CompositionalPhras> CompositionalPhrases { get; set; }

        [JsonProperty("antonyms")]
        public List<string> Antonyms { get; set; }
    }

    public class Result
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("headword")]
        public Headword Headword { get; set; }

        [JsonProperty("senses")]
        public List<Sens> Senses { get; set; }
    }

    public class Root
    {
        [JsonProperty("n_results")]
        public int NResults { get; set; }

        [JsonProperty("page_number")]
        public int PageNumber { get; set; }

        [JsonProperty("results_per_page")]
        public int ResultsPerPage { get; set; }

        [JsonProperty("n_pages")]
        public int NPages { get; set; }

        [JsonProperty("available_n_pages")]
        public int AvailableNPages { get; set; }

        [JsonProperty("results")]
        public List<Result> Results { get; set; }
    }
}
