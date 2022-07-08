using API.Tools;
using Business.Services.Implementation;
using Data.Models.Implementation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.ViewModels
{
    public class LargePaginatedListResponse<TEntity> : BaseResponseModel
        where TEntity : class
    {
        public LargePaginatedList<TEntity> Data { get; set; }
    }

    public class PaginatedListResponse<TEntity> : BaseResponseModel
        where TEntity : class
    {
        public PaginatedList<TEntity> Data { get; set; }
    }

    public class ListNewsResponseModel : BaseResponseModel
    {
        public PaginatedList<News> News { get; set; }
    }

    /// <summary>
    /// Contains the crucial information that should be returned by any dictionary APIs used.
    /// </summary>
    public class DictionaryAPIResponse : BaseResponseModel
    {
        public DictionaryAPIDTO Data { get; set; }
    }

    public class BaseResponseModel
    {
        public string Message { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }

    public class LoginRequestModel
    {
        [Required(ErrorMessage = "Field {0} is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Field {0} is required")]
        public string Password { get; set; }
    }

    public class RefreshRequestModel
    {
        [Required(ErrorMessage = "Field {0} is required")]
        public string ExpiredAccessToken { get; set; }
    }

    public class ResetPasswordRequestModel
    {
        [Required(ErrorMessage = "Field {0} is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Field {0} is required")]
        public string Token { get; set; }

        [Required(ErrorMessage = "Field {0} is required")]
        public string NewPassword { get; set; }
    }

    public class LoginResponseModel : BaseResponseModel
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }

    /// <summary>
    /// Class that mimics what was originally implemented in the front-end. There are many controllers that end up responding with basic metadata ('BaseResponseModel'), and the data of interest. This class aims to faciliate this setup.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataResponseModel<T> : BaseResponseModel
    {
        public T Data { get; set; }
    }
}
