using ImageSharingWithAuth.DataAccessLayer;
using ImageSharingWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageSharingWithAuth.Controllers
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
        public ActionResult Register(UserViewModel UserInfo)
        {
            SetIsAda();

            if (ModelState.IsValid)
            {
                User _user = DB.Users.SingleOrDefault(u => u.Userid.Equals(UserInfo.Userid));
                if (_user == null)
                {
                    //User data to database only ADA and Userid
                    _user = new User(UserInfo.Userid, UserInfo.ADA);
                    DB.Users.Add(_user);
                    DB.SaveChanges();
                    // Save it to Cookie
                    SaveCookie(UserInfo.Userid, UserInfo.ADA);
                    ViewBag.UserId = UserInfo.Userid;
                    // Registration success
                    return View("RegistrationResult");
                }
                else
                {
                    // If user already exist...
                    _user.ADA = UserInfo.ADA;
                    DB.Entry(_user).State = EntityState.Modified;
                    ViewBag.Message = "User already exist! Please Login";
                    return View();
                }

            }
            else
            {
                // Rediredct to Get : Registration
                return View();
            }
        }

        [HttpGet]
        public ActionResult Login()
        {
            SetIsAda();
            ViewBag.Message = string.Empty;
            return View();
        }

        [HttpPost]
        public ActionResult Login(string UserId)
        {
            SetIsAda();

            User _user = DB.Users.SingleOrDefault(x => x.Userid.Equals(UserId, StringComparison.OrdinalIgnoreCase));
            if (_user != null)
            {
                SaveCookie(UserId, _user.ADA);
                return View("RegistrationResult");
            }
            else
            {
                ViewBag.Message = $"No user with '{UserId}' found";
                return View("Login");
            }
        }

        // Method for saving the cookie
        protected void SaveCookie(String userid, bool ADA)
        {
            HttpCookie cookie = new HttpCookie("ImageSharing");
            cookie.Expires = DateTime.Now.AddMonths(3);
            cookie["ADA"] = ADA ? "true" : "false";
            cookie["UserId"] = userid;
            Response.Cookies.Add(cookie);
        }
    }
}