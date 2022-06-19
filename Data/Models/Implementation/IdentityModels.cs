using Data.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
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
        public virtual ICollection<UserClaim> Claims { get; set; } = new List<UserClaim>();
        public virtual ICollection<UserLogin> Logins { get; set; } = new List<UserLogin>();
        public virtual ICollection<UserToken> Tokens { get; set; } = new List<UserToken>();
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        public string Name { get; set; } = "";
        public string Surname { get; set; } = "";

        [JsonIgnore]
        public virtual ICollection<Deck> Decks { get; set; } = new List<Deck>();
        [JsonIgnore]
        public virtual ICollection<News> News { get; set; } = new List<News>();

        [NotMapped]
        public string DbId { get => Id; set => Id = value; }

        //public override bool Equals(object obj)
        //{
        //    return obj is User model &&
        //           Name == model.Name &&
        //           Surname == model.Surname &&
        //        NormalizedEmail == model.NormalizedEmail &&
        //        NormalizedUserName == model.NormalizedUserName;
        //}

        //public override int GetHashCode()
        //{
        //    return HashCode.Combine(Name, Surname, NormalizedEmail, NormalizedUserName);
        //}
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
