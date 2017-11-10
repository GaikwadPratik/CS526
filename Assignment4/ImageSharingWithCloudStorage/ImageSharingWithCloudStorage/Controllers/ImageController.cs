using ImageSharingWithCloudStorage.DataAccessLayer;
using ImageSharingWithCloudStorage.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageSharingWithCloudStorage.Controllers
{
    [Authorize]
    public class ImageController : BaseController
    {
        List<string> _lstAllowedExtensions = new List<string>()
        {
            "jpg", "jpeg"
        };

        // GET: Images
        [HttpGet]
        public ActionResult Upload()
        {
            SetIsAda();
            ViewBag.Message = string.Empty;
            SelectList _tags = new SelectList(ApplicationDbContext.Tags, "Id", "Name", 1);
            ViewBag.Tags = _tags;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(ImageViewModel Image, HttpPostedFileBase ImageFile)
        {
            SetIsAda();
            double _dSize = 4;
            TryValidateModel(Image);
            ViewBag.Tags = new SelectList(ApplicationDbContext.Tags, "Id", "Name", Image.TagId);
            if (ModelState.IsValid)
            {
                ApplicationUser _user = GetLoggedInUser();
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

                    if (ImageFile != null && ImageFile.ContentLength > 0 && ((double)ImageFile.ContentLength / (1024 * 1024)) > _dSize)
                    {
                        if (!_lstAllowedExtensions.Any(item => ImageFile.FileName.EndsWith(item, StringComparison.OrdinalIgnoreCase)))
                        {
                            ViewBag.ImageValidation = "File type must be jpg or jpeg";
                            return View();
                        }

                        ApplicationDbContext.Images.Add(_image);
                        ApplicationDbContext.SaveChanges();

                        string _strImageFileName = Server.MapPath($"~/Content/Images/img-{_image.Id}.jpg");
                        ImageFile.SaveAs(_strImageFileName);

                        ViewBag.tags = new SelectList(ApplicationDbContext.Tags, "Id", "Name", 1);
                        ViewBag.SuccessMessages = "Upload Successful";
                        ModelState.Clear();
                        return View();
                    }
                    else
                    {
                        ViewBag.Message = "No image file specified";
                        return View();
                    }
                }
                else
                {
                    ViewBag.Message = "No such user registered";
                    return View();
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
            SetIsAda();

            ViewBag.Message = string.Empty;
            return View();
        }

        [HttpGet]
        public ActionResult Details(int Id)
        {
            SetIsAda();
            Image _image = ApplicationDbContext.Images.Find(Id);
            bool _bIsPreview = false;
            if (TempData["Preview"] != null)
                bool.TryParse(TempData["Preview"].ToString(), out _bIsPreview);

            if (_image != null && (_image.IsApproved || _bIsPreview))
            {
                ImageViewModel _imageViewModel = new ImageViewModel()
                {
                    Id = _image.Id,
                    Caption = _image.Caption,
                    Description = _image.Description,
                    DateTaken = _image.DateTaken,
                    TagName = _image.Tag.Name,
                    UserId = _image.User.Email
                };
                //Adding the log entry.
                LogContext.addLogEntry(User.Identity.Name, _imageViewModel);
                return View(_imageViewModel);
            }
            else
                return View((ImageViewModel)null);
        }

        [HttpGet]
        public ActionResult Edit(int Id)
        {
            SetIsAda();
            Image _image = ApplicationDbContext.Images.Find(Id);
            if (_image != null)
            {
                ApplicationUser _user = GetLoggedInUser();
                if (_image.User.Email.Equals(_user.Email, StringComparison.OrdinalIgnoreCase))
                {
                    ViewBag.Message = string.Empty;
                    ViewBag.Tags = new SelectList(ApplicationDbContext.Tags, "Id", "Name", _image.TagId);
                    ImageViewModel _imageViewModel = new ImageViewModel()
                    {
                        Id = _image.Id,
                        // Fetching the image from the storage.
                        Uri = ImageStorage.ImageURI(Url, Id),
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
            {
                ViewBag.Message = $"Image with Identifier {Id} not found";
                ViewBag.Id = Id;
                return RedirectToAction("Error", "Home", new { errid = "EditNotAuth" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int Id, ImageViewModel ImageViewModel)
        {
            SetIsAda();
            ApplicationUser _user = GetLoggedInUser();
            try
            {
                Image _image = ApplicationDbContext.Images.Find(Id);
                ViewBag.Tags = new SelectList(ApplicationDbContext.Tags, "Id", "Name", _image.TagId);
                if (ModelState.IsValid)
                {
                    if (_image.User.Email.Equals(_user.Email))
                    {
                        ViewBag.Message = string.Empty;
                        _image.TagId = ImageViewModel.TagId;
                        _image.Caption = ImageViewModel.Caption;
                        _image.Description = ImageViewModel.Description;
                        _image.IsApproved = false;
                        _image.DateTaken = ImageViewModel.DateTaken;

                        ApplicationDbContext.Entry(_image).State = EntityState.Modified;
                        ApplicationDbContext.SaveChanges();

                        ViewBag.SuccessMessage = "Image changes saved";
                        TempData["Preview"] = true;
                        return RedirectToAction("Details", new { Id = Id });
                    }
                    else
                        return RedirectToAction("Error", "Home", new { errid = "EditNotAuth" });

                }
                else
                    return RedirectToAction("Error", "Home", new { errid = "EditNotFind" });

            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpGet]
        public ActionResult ListAll()
        {
            SetIsAda();
            ApplicationUser _user = GetLoggedInUser();
            ViewBag.Userid = _user.Email;
            return View(ApprovedImages);
        }

        [HttpGet]
        public ActionResult ListByUser()
        {
            SetIsAda();
            List<ApplicationUser> _activeUsers = ApplicationDbContext.Users.Where(x => x.IsActive).ToList();
            SelectList _users = new SelectList(_activeUsers, "Id", "Email", 1);
            return View(_users);
        }
        [HttpGet]
        public ActionResult DoListByUser(string Id)
        {
            SetIsAda();

            ApplicationUser _loggedInUser = GetLoggedInUser();
            ApplicationUser _user = ApplicationDbContext.Users.Find(Id);
            if (_user != null)
            {
                ViewBag.UserId = _user.Email;
                return View("ListAll", GetApprovedImages(_user.Images));
            }
            else
                return RedirectToAction("Error", "Home", new { errid = "ListByUser" });
        }
        [HttpGet]
        public ActionResult ListByTag()
        {
            SetIsAda();
            SelectList tags = new SelectList(ApplicationDbContext.Tags, "Id", "Name", 1);
            return View(tags);
        }
        [HttpGet]
        public ActionResult DoListByTag(int Id)
        {
            SetIsAda();
            ApplicationUser _user = GetLoggedInUser();
            Tag _tag = ApplicationDbContext.Tags.Find(Id);
            if (_tag != null)
            {
                ViewBag.UserId = _user.Email;
                return View("ListAll", GetApprovedImages(_tag.Images));
            }
            else
                return RedirectToAction("Error", "Home", new { errid = "ListByTag" });
        }
        [HttpGet]
        [Authorize(Roles = "Approver")]
        public ActionResult Approve()
        {
            SetIsAda();
            ViewBag.Message = string.Empty;
            return View(GetNonApprovedImages());
        }
        [HttpPost]
        [Authorize(Roles = "Approver")]
        [ValidateAntiForgeryToken]
        public ActionResult Approve(IList<SelectImageViewModel> model)
        {
            SetIsAda();
            ViewBag.Message = string.Empty;
            List<Image> _lstDeletedImages = new List<Image>();
            List<int> _lstDeletedImageIds = new List<int>();

            foreach (var _item in model)
            {
                Image _image = ApplicationDbContext.Images.Find(_item.Id);
                if (_image != null)
                {
                    if (!_image.IsApproved && _item.IsApproved && !_item.IsDeleted)
                    {
                        ApplicationDbContext.Images.Find(_item.Id).IsApproved = true;
                        ViewBag.Message = "Image(s) approved";
                    }
                    else if (!_image.IsApproved && !_item.IsApproved && _item.IsDeleted)
                    {
                        _lstDeletedImages.Add(_image);
                        _lstDeletedImageIds.Add(_image.Id);
                    }
                }
            }

            ApplicationDbContext.Images.RemoveRange(_lstDeletedImages);

            foreach (var _image in _lstDeletedImages)
            {
                ImageStorage.DeleteBlobs(_lstDeletedImageIds, Server);
            }

            ApplicationDbContext.SaveChanges();
            ViewBag.SuccessMessage = "Images are approved/deleted successfully";
            return View(GetNonApprovedImages());
        }
        [HttpGet]
        [Authorize(Roles = "Supervisor")]
        public ActionResult ImageList()
        {
            SetIsAda();
            DateTime date = DateTime.Now;
            List<SelectListItem> dateList = new List<SelectListItem>();
            for (int i = 0; i <= 13; i++)
            {
                dateList.Add(new SelectListItem
                {
                    Value = date.AddDays(-i).ToShortDateString(),
                    Text = date.AddDays(-i).ToShortDateString()
                });
                ViewBag.dateList = dateList;

            }
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Supervisor")]
        public ActionResult ImageViews(DateTime dateTaken)
        {
            SetIsAda();
            IEnumerable<LogEntry> allentries = LogContext.select(dateTaken);
            List<LogEntry> PrintEntries = new List<LogEntry>();
            foreach (var i in allentries)
            { PrintEntries.Add(i); }
            return View(PrintEntries.AsEnumerable<LogEntry>());
        }
        public ActionResult Delete(int Id)
        {
            SetIsAda();
            try
            {
                Image _image = ApplicationDbContext.Images.Find(Id);
                if (_image != null)
                {
                    ImageViewModel _imageViewModel = new ImageViewModel()
                    {
                        Id = _image.Id,
                        DateTaken = _image.DateTaken,
                        Description = _image.Description,
                        Caption = _image.Caption,
                        TagName = _image.Tag.Name,
                        UserId = _image.User.Email
                    };
                    return View(_imageViewModel);
                }
                else
                    return RedirectToAction("Error", "Home", new { errid = "Details" });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(FormCollection values, int Id)
        {
            SetIsAda();
            ApplicationUser _user = GetLoggedInUser();
            if (_user != null)
            {
                try
                {
                    Image _image = ApplicationDbContext.Images.Find(Id);
                    if (_image != null)
                    {
                        if (_image.User.Email.Equals(_user.Email, StringComparison.OrdinalIgnoreCase))
                        {
                            ApplicationDbContext.Images.Remove(_image);
                            ApplicationDbContext.SaveChanges();
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
                catch (Exception ex)
                {
                    return RedirectToAction("Error", "Home");
                }
            }
            else
                return RedirectToLogin();
        }

        private List<SelectImageViewModel> GetNonApprovedImages()
        {
            List<SelectImageViewModel> _lstModel = (from _dat in ApplicationDbContext.Images
                                                    where !_dat.IsApproved
                                                    select new SelectImageViewModel()
                                                    {
                                                        Id = _dat.Id,
                                                        Caption = _dat.Caption,
                                                        IsApproved = _dat.IsApproved
                                                    }).ToList();
            return _lstModel;
        }
    }
}