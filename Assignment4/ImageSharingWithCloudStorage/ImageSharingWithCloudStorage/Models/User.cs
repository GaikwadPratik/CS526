using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImageSharingWithCloudStorage.Models
{
    public class User
    {
        [Key]
        public virtual int Id { get; set; }
        [MaxLength(20)]
        public virtual string UserId { get; set; }
        public virtual bool ADA { get; set; }
        public virtual string UserName { get; set; }
        public virtual ICollection<Image> Images { get; set; }

        public User()
        {

        }

        public User(string u, bool a)
        {
            UserId = u;
            ADA = a;
            Images = new List<Image>();
        }

    }
}