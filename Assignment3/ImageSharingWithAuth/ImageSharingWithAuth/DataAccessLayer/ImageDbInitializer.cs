using ImageSharingWithAuth.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ImageSharingWithAuth.DataAccessLayer
{
    public class ImageDbInitializer:DropCreateDatabaseAlways<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext db)
        {
            RoleStore<IdentityRole> _roleStore = new RoleStore<IdentityRole>(db);
            UserStore<ApplicationUser> _userStore = new UserStore<ApplicationUser>(db);

            RoleManager<IdentityRole> _roleManager = new RoleManager<IdentityRole>(_roleStore);
            UserManager<ApplicationUser> _userManager = new UserManager<ApplicationUser>(_userStore);

            IdentityResult _identityResult = null;

            ApplicationUser _nobody = new ApplicationUser() { Email = "nobody@test.com", UserName = "nobody@test.com" };
            ApplicationUser _applicationUser = new ApplicationUser() { Email = "approver@test.com", UserName = "approver@test.com" };
            ApplicationUser _adminUser = new ApplicationUser() { Email = "admin@test.com", UserName = "admin@test.com" };

            _identityResult = _userManager.Create(_nobody, "test1234");
            _identityResult = _userManager.Create(_applicationUser, "test1234");
            _identityResult = _userManager.Create(_adminUser, "test1234");

            string _strUserRole = "User";
            _identityResult = _roleManager.Create(new IdentityRole(_strUserRole));
            if (!_userManager.IsInRole(_nobody.Id, _strUserRole))
                _identityResult = _userManager.AddToRole(_nobody.Id, _strUserRole);
            if (!_userManager.IsInRole(_applicationUser.Id, _strUserRole))
                _identityResult = _userManager.AddToRole(_applicationUser.Id, _strUserRole);
            if (!_userManager.IsInRole(_adminUser.Id, _strUserRole))
                _identityResult = _userManager.AddToRole(_adminUser.Id, _strUserRole);

            string _strAdminRole = "Admin";
            _identityResult = _roleManager.Create(new IdentityRole(_strAdminRole));
            if (!_userManager.IsInRole(_adminUser.Id, _strAdminRole))
                _identityResult = _userManager.AddToRole(_adminUser.Id, _strAdminRole);

            string _strApproverRole = "Approver";
            _identityResult = _roleManager.Create(new IdentityRole(_strApproverRole));
            if (!_userManager.IsInRole(_applicationUser.Id, _strApproverRole))
                _identityResult = _userManager.AddToRole(_applicationUser.Id, _strApproverRole);

            db.Tags.Add(new Tag { Name = "Festival" });
            db.Tags.Add(new Tag { Name = "SocialMediaUploads" });
            db.Tags.Add(new Tag { Name = "Nature Love" });
            db.Tags.Add(new Tag { Name = "Family" });
            db.Tags.Add(new Tag { Name = "Friends" });
            db.Tags.Add(new Tag { Name = "Commercial" });
            db.Tags.Add(new Tag { Name = "World" });

            db.SaveChanges();

            db.Images.Add(new Image
            {
                Caption = "India's biggest Festival.",
                Description = "This is India's biggest festival. This is festival of colors and light.",
                DateTaken = new DateTime(2015, 12, 23),
                UserId = _applicationUser.Id,
                TagId = 1,
                IsApproved = false
            });

            db.SaveChanges();
            base.Seed(db);
        }
    }
}