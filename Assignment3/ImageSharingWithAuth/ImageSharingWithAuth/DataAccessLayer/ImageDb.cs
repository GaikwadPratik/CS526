using ImageSharingWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ImageSharingWithAuth.DataAccessLayer
{
    public class ImageDb : DbContext
    {
        public ImageDb() :
            base("ImageSharingWithAuth")
        {

        }
        public DbSet<Image> Images { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Tag> Tags { get; set; }
    }
}