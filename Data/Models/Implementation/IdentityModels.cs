using Data.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xunit.Abstractions;

namespace Data.Models.Implementation
{
    // following implementation follows .NET boilerplate for identity customization (https://docs.microsoft.com/en-us/aspnet/core/security/authentication/customize-identity-model?view=aspnetcore-5.0)
    public class User : IdentityUser, IDatabaseItem<string>
    {
        public virtual ICollection<UserClaim> Claims { get; set; }
        public virtual ICollection<UserLogin> Logins { get; set; }
        public virtual ICollection<UserToken> Tokens { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }

        public string Name { get; set; } = "";
        public string Surname { get; set; } = "";

        [NotMapped]
        public string DbId { get => Id; set => Id = value; }
    }

    public class Role : IdentityRole, IDatabaseItem<string>
    {
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public virtual ICollection<RoleClaim> RoleClaims { get; set; } = new List<RoleClaim>();

        [NotMapped]
        public string DbId { get => Id; set => Id = value; }
    }

    public class UserRole : IdentityUserRole<string>
    {
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }

    public class UserClaim : IdentityUserClaim<string>
    {
        public virtual User User { get; set; }
    }

    public class UserLogin : IdentityUserLogin<string>
    {
        public virtual User User { get; set; }
    }

    public class RoleClaim : IdentityRoleClaim<string>
    {
        public virtual Role Role { get; set; }
    }

    public class UserToken : IdentityUserToken<string>
    {
        public virtual User User { get; set; }
    }
}
