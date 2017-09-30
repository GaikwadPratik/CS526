using ImageSharingWithModel.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ImageSharingWithModel.DataAccessLayer
{
    public class ImageDbInitializer:DropCreateDatabaseAlways<ImageDb>
    {
        protected override void Seed(ImageDb db)
        {
            db.Users.Add(new User { Userid = "admin", ADA = false });
            db.Users.Add(new User { Userid = "student", ADA = false });

            db.Tags.Add(new Tag { name = "Festival" });
            db.Tags.Add(new Tag { name = "SocialMediaUploads" });
            db.Tags.Add(new Tag { name = "Nature Love" });
            db.Tags.Add(new Tag { name = "Family" });
            db.Tags.Add(new Tag { name = "Friends" });
            db.Tags.Add(new Tag { name = "Commercial" });
            db.Tags.Add(new Tag { name = "World" });

            db.SaveChanges();

            db.Images.Add(new Image
            {
                Caption = "India's biggest Festival.",
                Description = "This is India's biggest festival. This is festival of colors and light.",
                DateTaken = new DateTime(2015, 12, 23),
                UserId = 1,
                TagId = 1
            });

            db.Images.Add(new Image
            {
                Caption = "Natural",
                Description = "View from my house.",
                DateTaken = new DateTime(2012, 12, 12),
                UserId = 2,
                TagId = 2
            });
            db.SaveChanges();
            base.Seed(db);
        }
    }
}