using Data.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using Xunit.Abstractions;

namespace Data.Models.Implementation
{
    // following implementation follows .NET boilerplate for identity customization (https://docs.microsoft.com/en-us/aspnet/core/security/authentication/customize-identity-model?view=aspnetcore-5.0)
    public class ApplicationUser : IdentityUser, IDatabaseItem<string>
    {
        public virtual ICollection<ApplicationUserClaim> Claims { get; set; }
        public virtual ICollection<ApplicationUserLogin> Logins { get; set; }
        public virtual ICollection<ApplicationUserToken> Tokens { get; set; }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }

        public string DbId { get => Id; set => Id = value; }

        /// <summary>
        /// According to the internet: "The Deserialize method is, I think, used if you have your test cases stored in an external file that is read in by XUnit.  For our case, it doesn’t matter, so we can just leave it blank". Source: https://darchuk.net/2019/04/12/serializing-xunit-test-cases/.
        /// </summary>
        /// <param name="info"></param>
        public void Deserialize(IXunitSerializationInfo info) { }

        public void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue(nameof(Id), Id);
            info.AddValue(nameof(UserName), UserName);
            info.AddValue(nameof(Email), Email);
        }
    }

    public class ApplicationRole : IdentityRole, IDatabaseItem<string>
    {
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; } = new List<ApplicationUserRole>();
        public virtual ICollection<ApplicationRoleClaim> RoleClaims { get; set; } = new List<ApplicationRoleClaim>();

        public string DbId { get => Id; set => Id = value; }

        /// <summary>
        /// According to the internet: "The Deserialize method is, I think, used if you have your test cases stored in an external file that is read in by XUnit.  For our case, it doesn’t matter, so we can just leave it blank". Source: https://darchuk.net/2019/04/12/serializing-xunit-test-cases/.
        /// </summary>
        /// <param name="info"></param>
        public void Deserialize(IXunitSerializationInfo info) { }

        public void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue(nameof(Id), Id);
            info.AddValue(nameof(Name), Name);
        }
    }

    public class ApplicationUserRole : IdentityUserRole<string>
    {
        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationRole Role { get; set; }
    }

    public class ApplicationUserClaim : IdentityUserClaim<string>
    {
        public virtual ApplicationUser User { get; set; }
    }

    public class ApplicationUserLogin : IdentityUserLogin<string>
    {
        public virtual ApplicationUser User { get; set; }
    }

    public class ApplicationRoleClaim : IdentityRoleClaim<string>
    {
        public virtual ApplicationRole Role { get; set; }
    }

    public class ApplicationUserToken : IdentityUserToken<string>
    {
        public virtual ApplicationUser User { get; set; }
    }
}
