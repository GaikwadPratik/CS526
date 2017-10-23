using ImageSharingWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageSharingWithAuth.Controllers
{
    public class HomeController : BaseController
    {
        [AllowAnonymous]
        [RequireHttps]
        public ActionResult Index(string id = "Stranger")
        {
            SetIsAda();
            ViewBag.Title = $"Welcome ";
            ApplicationUser _user = GetLoggedInUser();
            ViewBag.Id = _user != null ? _user.UserName : id;
            return View();
        }

        public ActionResult Error(string errid = "Unspecified")
        {
            if ("Details".Equals(errid, StringComparison.OrdinalIgnoreCase))
                ViewBag.Message = "Problem with Details action!";
            else
                ViewBag.Message = "Unspecified error!";
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}