using API.Tools;
using Data.Models.Implementation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.ViewModels
{
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
        // to be determined/implemented
    }

    public class BaseResponseModel
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }

    public class RegisterRequestModel
    {
        [Required(ErrorMessage = "Field {0} is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Field {0} is required")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Field {0} is required")]
        [EmailAddress(ErrorMessage = "Field {0} format is invalid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Field {0} is required")]
        [StringLength(100, ErrorMessage = "Field {0} requires an amount of characters between {2} and {1}", MinimumLength = 6)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Password does not match")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginRequestModel
    {
        [Required(ErrorMessage = "Field {0} is required")]
        [EmailAddress(ErrorMessage = "Field {0} format is invalid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Field {0} is required")]
        [StringLength(100, ErrorMessage = "Field {0} requires an amount of characters between {2} and {1}", MinimumLength = 6)]
        public string Password { get; set; }
    }

    public class LoginResponseModel : BaseResponseModel
    {
        public string JWTToken { get; set; }
    }
}
