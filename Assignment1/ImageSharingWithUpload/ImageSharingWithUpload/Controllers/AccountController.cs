using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageSharingWithUpload.Controllers
{
    public class AccountController : BaseController
    {
        // GET: Account
        [HttpGet]
        public ActionResult Register()
        {
            SetIsAda();
            return View();
        }

        [HttpPost]
        public ActionResult Register(string txtUserId, bool chkAda)
        {
            ViewBag.UserId = txtUserId;
            HttpCookie _cookie = new HttpCookie("ImageSharing");
            _cookie.Expires = DateTime.Now.AddMinutes(3);
            _cookie["UserId"] = txtUserId;
            _cookie["IsAda"] = chkAda ? "true" : "false";
            Response.Cookies.Add(_cookie);
            SetIsAda();
            return View("RegistrationResult");
        }
    }
}