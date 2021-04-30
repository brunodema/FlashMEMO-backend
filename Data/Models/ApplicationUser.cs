using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public IList<string> UserRoles { get; set; }
    }
}
