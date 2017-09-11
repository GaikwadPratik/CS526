using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageSharingWithUpload.Controllers
{
    public class BaseController : Controller
    {
        protected bool CheckUserRegistration
        {
            get
            {
                HttpCookie _cookie = Request.Cookies["ImageSharing"];
                bool _rtnVal = false;

                if (_cookie != null)
                {
                    if (_cookie["UserId"] != null && _cookie["UserId"] != string.Empty)
                        _rtnVal = true;
                }

                return _rtnVal;
            }
        }

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

        
    }
}