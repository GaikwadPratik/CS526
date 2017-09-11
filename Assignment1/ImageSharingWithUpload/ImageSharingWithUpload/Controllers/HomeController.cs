using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageSharingWithUpload.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index(string id = "Stranger")
        {
            SetIsAda();
            ViewBag.Id = id;
            return View();
        }        
    }
}