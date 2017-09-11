using ImageSharingWithUpload.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageSharingWithUpload.Controllers
{
    public class ImagesController : BaseController
    {
        List<string> _lstAllowedExtensions = new List<string>()
        {
            "jpg", "jpeg"
        };
        // GET: Images
        [HttpGet]
        public ActionResult Upload()
        {
            if (CheckUserRegistration)
            {
                SetIsAda();
                return View();
            }
            else
            {
                ViewBag.Message = "Please register before uploading";
                return View("../Account/Register");
            }
        }

        [HttpPost]
        public ActionResult Upload(Image Image, HttpPostedFileBase ImageFile)
        {
            SetIsAda();
            if (ModelState.IsValid)
            {
                HttpCookie _cookie = Request.Cookies.Get("ImageSharing");
                if (_cookie != null)
                {
                    Image.UserId = _cookie["UserId"]; //TODO:: Set this from UI 

                    if (ImageFile != null && ImageFile.ContentLength > 0)
                    {
                        string _strFileExtension = Path.GetExtension(ImageFile.FileName).Substring(1);
                        //file size should be less than 100KB
                        if (ImageFile.ContentLength > 100000)
                        {
                            ModelState.AddModelError("ImageSize", "The size of the image should not exceed 100 KB");
                            return View();
                        }
                        else if (!_lstAllowedExtensions.Contains(_strFileExtension))
                        {
                            ModelState.AddModelError("ImageExtension", "Only (jpg, jpeg) extensions are supported");
                            return View();
                        }
                        else
                        {
                            try
                            {
                                string _strServerImageFile = Server.MapPath($"~/Content/Images/{Image.Id}.jpg");
                                ImageFile.SaveAs(_strServerImageFile);
                                string _jsonData = JsonConvert.SerializeObject(Image);
                                System.IO.File.WriteAllText(Server.MapPath($"~/App_Data/Image_Info/{Image.Id}.json"), _jsonData);
                                ModelState.Clear();
                                ViewBag.Message = "Image successfully uploaded";
                            }
                            catch (Exception ex)
                            {
                                ViewBag.Exception = ex;
                                //Render error view
                                return View("../Shared/Error");
                            }
                        }
                    }
                    return View();
                }
                else
                {
                    ViewBag.Message = "Please register before uploading";
                    return View("../Account/Register");
                }
            }
            else
            {
                ViewBag.Message = "Please correct the errors on the page";
                return View();
            }
        }

        [HttpGet]
        public ActionResult Query()
        {
            if (CheckUserRegistration)
            {
                SetIsAda();
                ViewBag.Message = string.Empty;
                return View();
            }
            else
            {
                ViewBag.Message = "Please register before making a query";
                return View("../Account/Register");
            }
        }

        [HttpGet]
        public ActionResult Details(string id)
        {
            SetIsAda();
            HttpCookie _cookie = Request.Cookies.Get("ImageSharing");
            if (_cookie != null)
            {
                string _jsonFileName = Server.MapPath($"~/App_data/Image_Info/{id}.json");
                string _imageFileName = Server.MapPath($"~/Content/Images/{id}.jpg");
                if (System.IO.File.Exists(_jsonFileName) && System.IO.File.Exists(_imageFileName))
                {
                    Image _image = JsonConvert.DeserializeObject<Image>(System.IO.File.ReadAllText(_jsonFileName));
                    return View("QueryResponse", _image);
                }
                else
                {
                    ViewBag.Message = $"Image identifier with Id: {id} not found on server";
                    return View("Query");
                }
            }
            else
            {
                ViewBag.Message = "Please register before making a query";
                return View("../Account/Register");
            }
        }
    }
}