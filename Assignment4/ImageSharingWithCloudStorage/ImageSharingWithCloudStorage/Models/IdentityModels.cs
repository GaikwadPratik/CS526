﻿using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace ImageSharingWithCloudStorage.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public virtual bool ADA { get; set; }
        public virtual bool IsActive { get; set; }

        public virtual ICollection<Image> Images { get; set; }

        public ApplicationUser()
        {
            IsActive = true;
        }

        public ApplicationUser(string userName, bool Ada)
        {
            IsActive = true;
            UserName = userName;
            ADA = Ada;
            Images = new List<Image>();
        }
    }
}