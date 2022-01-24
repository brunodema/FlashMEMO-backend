using Data.Models.Implementation;

namespace Business.Services.Interfaces
{
    public interface IImageAPIServiceOptions
    {
        string BaseURL { get; set; }
        string EngineID { get; set; }
        string Token { get; set; }
    }
    public class ImageAPIServiceOptions : IImageAPIServiceOptions
    {
        public string BaseURL { get; set; }
        public string EngineID { get; set; }
        public string Token { get; set; }
    }
    public interface IImageAPIService
    {
        public string SearchImage();
    }
}
