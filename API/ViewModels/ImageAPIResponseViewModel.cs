using Google.Apis.CustomSearchAPI.v1.Data;
using System.Collections.Generic;
using static Google.Apis.CustomSearchAPI.v1.Data.Result;

namespace API.ViewModels
{
    public class ImageAPIResponseViewModel
    {
        public string Title { get; set; }
        public ImageData Image { get; set; }
        public string Link { get; set; }

        public ImageAPIResponseViewModel(string title, ImageData imageData, string link)
        {
            Title = title;
            Image = imageData;
            Link = link;
        }
    }
}