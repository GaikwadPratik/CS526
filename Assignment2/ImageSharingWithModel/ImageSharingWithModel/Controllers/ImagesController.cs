using ImageSharingWithModel.DataAccessLayer;
using ImageSharingWithModel.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageSharingWithModel.Controllers
{
    public class ImagesController : BaseController
    {
        List<string> _lstAllowedExtensions = new List<string>()
        {
            "jpg", "jpeg"
        };

        private ImageDb _db = new ImageDb();

        // GET: Images
        [HttpGet]
        public ActionResult Upload()
        {
            SetIsAda();
            User _user = GetLoggedInUser();
            if (_user == null)
                return RedirectToLogin();
            else
            {
                ViewBag.Message = string.Empty;
                ViewBag.Tags = new SelectList(_db.Tags, "Id", "Name", 1);
                return View();
            }
        }

        [HttpPost]
        public ActionResult Upload(ImageViewModel Image, HttpPostedFileBase ImageFile)
        {
            SetIsAda();
            TryValidateModel(Image);
            ViewBag.Tags = new SelectList(_db.Tags, "Id", "Name", 1);
            if (ModelState.IsValid)
            {
                User _user = GetLoggedInUser();

                if (_user != null)
                {
                    Image _image = new Models.Image()
                    {
                        DateTaken = Image.DateTaken,
                        Caption = Image.Caption,
                        Description = Image.Description,
                        User = _user,
                        TagId = Image.TagId
                    };

                    if (ImageFile != null && ImageFile.ContentLength > 0)
                    {
                        if (!_lstAllowedExtensions.Any(item => ImageFile.FileName.EndsWith(item, StringComparison.OrdinalIgnoreCase)))
                        {
                            ViewBag.ImageValidation = "File type must be jpg or jpeg";
                            return View();
                        }

                        double _dFileSize = 1;
                        if (((double)ImageFile.ContentLength / (1024 * 1024)) > _dFileSize)
                        {
                            ViewBag.ImageValidation = "File size is greater";
                            return View();
                        }

                        DB.Images.Add(_image);
                        DB.SaveChanges();

                        string imgFileName = Server.MapPath("~/Content/Images/img-" + _image.Id + ".jpg");
                        ImageFile.SaveAs(imgFileName);
                        return RedirectToAction("Details", new { Id = _image.Id });
                    }
                    else
                        //If issue with image upload
                        return RedirectToAction("Error", "Home", new { errid = "Upload" });
                }
                else
                    return RedirectToLogin();
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
            SetIsAda();
            if (GetLoggedInUser() != null)
            {
                ViewBag.Message = string.Empty;
                return View();
            }
            else
            {
                return RedirectToLogin();
            }
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            SetIsAda();
            if (GetLoggedInUser() == null)
                return RedirectToLogin();
            else
            {
                Image _image = DB.Images.Find(id);
                if (_image != null)
                {
                    ImageViewModel _imageViewModel = new ImageViewModel()
                    {
                        Id = _image.Id,
                        Caption = _image.Caption,
                        Description = _image.Description,
                        DateTaken = _image.DateTaken,
                        TagName = _image.Tag.name,
                        UserId = _image.User.Userid
                    };
                    return View(_imageViewModel);
                }
                else
                    return RedirectToAction("Error", "Home", new { errid = "Details" });
            }
        }

        [HttpGet]
        public ActionResult Edit(int Id)
        {
            SetIsAda();
            User _user = GetLoggedInUser();
            if (_user == null)
                return RedirectToLogin();
            else
            {
                Image _image = DB.Images.Find(Id);
                if (_image != null)
                {
                    if (_image.User.Userid.Equals(_user.Userid, StringComparison.OrdinalIgnoreCase))
                    {
                        ViewBag.Message = string.Empty;
                        ViewBag.Tags = new SelectList(DB.Tags, "Id", "name", _image.TagId);
                        ImageViewModel _imageViewModel = new ImageViewModel()
                        {
                            Id = _image.Id,
                            TagId = _image.TagId,
                            Caption = _image.Caption,
                            Description = _image.Description,
                            DateTaken = _image.DateTaken
                        };
                        return View("Edit", _imageViewModel);
                    }
                    else
                        return RedirectToAction("Error", "Home", new { errid = "EditNotAuth" });
                }
                else
                    return RedirectToAction("Error", "Home", new { errid = "EditNotFound" });
            }
        }

        [HttpPost]
        public ActionResult Edit(int Id, ImageViewModel ImageViewModel)
        {
            SetIsAda();
            User _user = GetLoggedInUser();
            if (_user == null)
                return RedirectToLogin();
            else
            {
                Image _image = DB.Images.Find(Id);
                ViewBag.Tags = new SelectList(DB.Tags, "Id", "Name", _image.TagId);

                if (ModelState.IsValid)
                {
                    if (_image != null)
                    {
                        if (_image.User.Userid.Equals(_user.Userid, StringComparison.OrdinalIgnoreCase))
                        {
                            _image.TagId = ImageViewModel.TagId;
                            _image.Caption = ImageViewModel.Caption;
                            _image.Description = ImageViewModel.Description;
                            _image.DateTaken = ImageViewModel.DateTaken;
                            DB.Entry(_image).State = System.Data.Entity.EntityState.Modified;
                            DB.SaveChanges();
                            return RedirectToAction("Details", new { Id = Id });
                        }
                        else
                        {
                            return RedirectToAction("Error", "Home", new { errid = "EditNotAuth" });
                        }
                    }
                    else
                        return RedirectToAction("Error", "Home", new { errid = "EditNotFound" });
                }
                else
                {
                    ViewBag.Message = "Please Correct All the errors";
                    _image.Id = Id;
                    return View("Edit", _image);
                }
            }
        }

        [HttpGet]
        public ActionResult Delete(int Id)
        {
            SetIsAda();
            User _user = GetLoggedInUser();
            if (_user == null)
                return RedirectToLogin();
            else
            {
                Image _image = DB.Images.Find(Id);
                if (_image != null)
                {
                    ImageViewModel _imageViewModel = new ImageViewModel()
                    {
                        Id = _image.Id,
                        DateTaken = _image.DateTaken,
                        Description = _image.Description,
                        Caption = _image.Caption,
                        TagName = _image.Tag.name,
                        UserId = _image.User.Userid
                    };
                    return View(_imageViewModel);
                }
                else
                    return RedirectToAction("Error", "Home", new { errid = "Details" });
            }
        }

        [HttpPost]
        public ActionResult Delete(FormCollection values, int Id)
        {
            SetIsAda();
            User _user = GetLoggedInUser();
            if (User == null)
                return RedirectToLogin();
            else
            {
                Image _image = DB.Images.Find(Id);
                if (_image != null)
                {
                    if (_image.User.Userid.Equals(_user.Userid, StringComparison.OrdinalIgnoreCase))
                    {
                        DB.Images.Remove(_image);
                        DB.SaveChanges();
                        return RedirectToAction("Index", "Home");
                    }
                    else
                        return RedirectToAction("Error", "Home", new { errid = "DeleteNotAuth" });
                }
                else
                {
                    return RedirectToAction("Error", "Home", new { errid = "DeleteNotFound" });
                }
            }
        }

        [HttpGet]
        public ActionResult ListByUser()
        {
            SetIsAda();
            SelectList _users = new SelectList(DB.Users, "Id", "Userid", 1);
            User _user = GetLoggedInUser();
            if (_user == null)
                return RedirectToLogin();
            else
            {
                ViewBag.Userid = _user.Userid;
                return View(_users);
            }
        }

        [HttpGet]
        public ActionResult DoListByUser(int Id)
        {
            SetIsAda();

            User _user = GetLoggedInUser();
            if (_user == null)
                return RedirectToLogin();
            else
            {
                ViewBag.Userid = _user.Userid;
                return View("ListAll", _user.Images);
            }
        }

        [HttpGet]
        public ActionResult ListAll()
        {
            SetIsAda();
            List<Image> _images = DB.Images.ToList();
            User _user = GetLoggedInUser();
            if (_user == null)
                return RedirectToLogin();
            else
            {
                ViewBag.Userid = _user.Userid;
                return View(_images);
            }
        }

        [HttpGet]
        public ActionResult DoListByTag(int Id)
        {
            SetIsAda();
            User _user = GetLoggedInUser();
            if (_user == null)
                return RedirectToLogin();
            else
            {
                Tag _tag = DB.Tags.Find(Id);
                if (_tag != null)
                {
                    ViewBag.Userid = _user.Userid;
                    return View("ListAll", _tag.Images);
                }
                else
                    return RedirectToAction("Error", "Home", new { errid = "ListByTag" });
            }
        }

        [HttpGet]
        public ActionResult ListByTag()
        {
            SetIsAda();
            SelectList tags = new SelectList(DB.Tags, "Id", "Name", 1);
            User _user = GetLoggedInUser();
            if (_user == null)
                return RedirectToLogin();
            else
            {
                ViewBag.Userid = _user.Userid;
                return View(tags);
            }

        }
    }
}