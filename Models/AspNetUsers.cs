using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Noclegi.Model
{
    public partial class AspNetUsers
    {
        public AspNetUsers()
        {
            //AspNetAdvertisement = new HashSet<AspNetAdvertisement>();
            //AspNetUserClaims = new HashSet<AspNetUserClaims>();
            //AspNetUserLanguage = new HashSet<AspNetUserLanguage>();
            //AspNetUserLogins = new HashSet<AspNetUserLogins>();
            //AspNetUserRoles = new HashSet<AspNetUserRoles>();
            //AspNetUserTokens = new HashSet<AspNetUserTokens>();
        }

        public string Id { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastLoginDate { get; set; }

       // public virtual ICollection<AspNetAdvertisement> AspNetAdvertisement { get; set; }
       // public virtual ICollection<AspNetUserClaims> AspNetUserClaims { get; set; }
       // public virtual ICollection<AspNetUserLanguage> AspNetUserLanguage { get; set; }
       // public virtual ICollection<AspNetUserLogins> AspNetUserLogins { get; set; }
       // public virtual ICollection<AspNetUserRoles> AspNetUserRoles { get; set; }
       // public virtual ICollection<AspNetUserTokens> AspNetUserTokens { get; set; }
    }
}
