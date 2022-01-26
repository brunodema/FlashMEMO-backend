using Business.Services.Interfaces;
using Business.Tools;
using Google.Apis.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Google.Apis.CustomSearchAPI.v1.CseResource;
using static Google.Apis.CustomSearchAPI.v1.Data.Result;

namespace Business.Services.Implementation
{
    // no interfaces for this one because there are no other viable replacements for this API at the moment

    public class CustomSearchAPIImageResult
    {
        public string Title { get; set; }
        public ImageData Image { get; set; }
        public string Link { get; set; }

        public CustomSearchAPIImageResult(string title, ImageData imageData, string link)
        {
            Title = title;
            Image = imageData;
            Link = link;
        }
    }

    public class CustomSearchAPIResponse
    {
        public IEnumerable<CustomSearchAPIImageResult> Results { get; set; }
        public int ResultSize { get; set; }
        public UInt64 PageIndex { get; set; }
        public UInt64 TotalAmount { get; set; }
        public UInt64 TotalPages { get; set; }
    }

    public class CustomSearchAPIServiceOptions
    {
        public string BaseURL { get; set; }
        public string EngineID { get; set; }
        public string Token { get; set; }
    }
    public class CustomSearchAPIService : IAPIService
    {
        private readonly CustomSearchAPIServiceOptions _options;

        public CustomSearchAPIService(IOptions<CustomSearchAPIServiceOptions> options)
        {
            _options = options.Value;
        }

        public HttpResponse CheckAvailability()
        {
            throw new NotImplementedException();
        }

        public HttpResponse CheckPeriodComsumption()
        {
            throw new NotImplementedException();
        }

        private bool IsSearchTextValid(string searchText)
        {
            return !String.IsNullOrEmpty(searchText);
        }

        private List<string> IsInputValid(string searchText, int pageSize, int pageNumber)
        {
            var errorMessages = new List<string>();

            if (!IsSearchTextValid(searchText))
            {
                errorMessages.Add("The search text used is not valid. It is either 'null' or empty.");
            }
            if (pageSize <= 0 || pageSize > 10) // the GT part is a restriction of the Google API itself
            {
                errorMessages.Add("The page size has an invalid number (less or equal than 0, or greater than 10).");
            }
            if (pageNumber <= 0)
            {
                errorMessages.Add("The page size has an invalid number (less or equal than 0).");
            }

            return errorMessages;
        }

        public async Task<CustomSearchAPIResponse> Search(string searchText, int pageSize = 10, int pageNumber = 1)
        {
            try
            {
                using (var service = new Google.Apis.CustomSearchAPI.v1.CustomSearchAPIService(new BaseClientService.Initializer { ApplicationName = "FlashMEMO Image Search", ApiKey = _options.Token }))
                {
                    ListRequest listRequest = service.Cse.List();

                    var validationsMessages = IsInputValid(searchText, pageSize, pageNumber);
                    if (validationsMessages.Count > 0)
                    {
                        throw new InputValidationException()
                        {
                            InputValidationErrors = validationsMessages
                        };
                    } 

                    listRequest.Q = searchText;
                    listRequest.SearchType = ListRequest.SearchTypeEnum.Image;
                    listRequest.Cx = _options.EngineID;
                    listRequest.Start = (pageSize * pageNumber) - pageSize;
                    listRequest.Num = pageSize;
                    var results = await listRequest.ExecuteAsync();

                    var totalAmount = Convert.ToUInt64(results?.SearchInformation.TotalResults ?? "0");
                    return new CustomSearchAPIResponse
                    {
                            Results = results.Items.Select(i => new CustomSearchAPIImageResult(i.Title, i.Image, i.Link)).ToList(),
                            ResultSize = results?.Items?.Count ?? 0,
                            PageIndex = Convert.ToUInt64(pageNumber),
                            TotalAmount = totalAmount,
                            TotalPages = Convert.ToUInt64(Math.Ceiling(totalAmount / (double)pageSize))

                    };
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
