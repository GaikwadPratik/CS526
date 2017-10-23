using ImageSharingWithAuth.DataAccessLayer;
using ImageSharingWithAuth.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageSharingWithAuth.Controllers
{
    public class BaseController : Controller
    {
        protected ApplicationDbContext ApplicationDbContext { get; set; }
        protected UserManager<ApplicationUser> UsersManager { get; set; }
        protected IEnumerable<ApplicationUser> ActiveUsers
        {
            get
            {
                return ApplicationDbContext.Users.Where(u => u.IsActive);
            }
        }
        protected IEnumerable<Image> ApprovedImages
        {
            get
            {
                return GetApprovedImages(ApplicationDbContext.Images);
            }
        }

        protected BaseController()
        {
            ApplicationDbContext = new ApplicationDbContext();
            UsersManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(ApplicationDbContext));
        }

        protected void SetIsAda()
        {
            try
            {
                HttpCookie _cookie = Request.Cookies["ImageSharing"];
                bool _isAda = false;

                if (_cookie != null)
                {
                    if (_cookie["IsAda"] != null && _cookie["IsAda"].Equals("true", StringComparison.OrdinalIgnoreCase))
                        _isAda = true;
                }

                ViewBag.IsAda = _isAda;
            }
            catch(Exception ex)
            {
                RedirectToAction("Error", "Home");
            }
        }

        protected void SaveCookie(bool ADA)
        {
            HttpCookie _cookie = new HttpCookie("ImageSharing")
            {
                Expires = DateTime.Now.AddMinutes(10),
                HttpOnly = true,
            };
            _cookie["ADA"] = ADA.ToString();
            Response.Cookies.Add(_cookie);
        }

        protected IEnumerable<Image> GetApprovedImages(IEnumerable<Image> images)
        {
            return images.Where(i => i.IsApproved);
        }

        protected SelectList UserSelectList()
        {
            string _strDefaultId = GetLoggedInUser().Id;
            return new SelectList(ActiveUsers, "Id", "UserName", _strDefaultId);
        }

        protected ApplicationUser GetLoggedInUser()
        {
            return UsersManager.FindById(User.Identity.GetUserId());
        }

        protected ActionResult RedirectToLogin()
        {
            return RedirectToAction("Login", "Account");
        }
    }
}