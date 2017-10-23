using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImageSharingWithAuth.Models
{
    public class Tag
    {
        [Key]
        public virtual int Id { get; set; }
        [MaxLength(20)]
        public virtual string Name { get; set; }
        public virtual ICollection<Image> Images { get; set; }

        public Tag()
        {

        }

        public Tag(string name)
        {
            Name = name;
        }
    }
}