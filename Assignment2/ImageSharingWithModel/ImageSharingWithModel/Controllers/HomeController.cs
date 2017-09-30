using ImageSharingWithModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageSharingWithModel.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index(string id = "Stranger")
        {
            SetIsAda();
            ViewBag.Title = "Welcome";
            User _loggedInUserId = GetLoggedInUser();
            ViewBag.Id = _loggedInUserId != null ? _loggedInUserId.Userid : id;
            return View();
        }

        public ActionResult Error(String errid = "Unspecified")
        {
            SetIsAda();
            if ("Details".Equals(errid))
            {
                ViewBag.Message = "Problem With Details Action";
            }
            else if ("Upload".Equals(errid))
            {
                ViewBag.Message = "Problem With Upload Action";
            }
            else if ("UploadNotAuth".Equals(errid))
            {
                ViewBag.Message = "Problem With Upload Action";
            }
            else if ("EditNotAuth".Equals(errid))
            {
                ViewBag.Message = "Problem With Edit Action";
            }
            else if ("EditNotFound".Equals(errid))
            {
                ViewBag.Message = "Problem With Edit Action";
            }

            else if ("DeleteNotAuth".Equals(errid))
            {
                ViewBag.Message = "Problem With Delete Action";
            }
            else if ("DeleteNotFound".Equals(errid))
            {
                ViewBag.Message = "Problem With Delete Action";
            }
            else if ("ListByUser".Equals(errid))
            {
                ViewBag.Message = "Problem With ListByUser Action";
            }
            else if ("ListByTag".Equals(errid))
            {
                ViewBag.Message = "Problem With ListByTag Action";
            }
            else
            {
                ViewBag.Message = "Unspecified Error";
            }
            return View();
        }
    }
}