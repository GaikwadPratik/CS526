using ImageSharingWithAuth.DataAccessLayer;
using ImageSharingWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageSharingWithAuth.Controllers
{
    public class BaseController : Controller
    {
        ImageDb _db = new ImageDb();

        public ImageDb DB
        {
            get { return _db; }
        }

        //protected bool CheckUserRegistration
        //{
        //    get
        //    {
        //        HttpCookie _cookie = Request.Cookies["ImageSharing"];
        //        bool _rtnVal = false;

        //        if (_cookie != null)
        //        {
        //            if (_cookie["UserId"] != null && _cookie["UserId"] != string.Empty)
        //                _rtnVal = true;
        //        }

        //        return _rtnVal;
        //    }
        //}

        protected void SetIsAda()
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

        protected User GetLoggedInUser()
        {
            User _user = null;
            HttpCookie cookie = Request.Cookies.Get("ImageSharing");
            if (cookie != null && cookie["Userid"] != null)
            {
                string _userId = cookie["Userid"] as string;
                if (!string.IsNullOrEmpty(_userId))
                {
                    _user = _db.Users.SingleOrDefault(x => x.Userid.Equals(_userId, StringComparison.OrdinalIgnoreCase));
                }
            }
            return _user;
        }

        protected ActionResult RedirectToLogin()
        {
            return RedirectToAction("Login", "Account");
        }
    }
}