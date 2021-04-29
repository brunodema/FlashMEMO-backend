using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.ViewModels
{
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

    //public class UserTokenViewModel
    //{
    //    public string Id { get; set; }
    //    public string Email { get; set; }
    //    public IEnumerable<ClaimViewModel> Claims { get; set; }
    //}

    //public class LoginResponseViewModel
    //{
    //    public string AccessToken { get; set; }
    //    public double ExpiresIn { get; set; }
    //    public UserTokenViewModel UserToken { get; set; }
    //}

    //public class ClaimViewModel
    //{
    //    public string Value { get; set; }
    //    public string Type { get; set; }
    //}
}
