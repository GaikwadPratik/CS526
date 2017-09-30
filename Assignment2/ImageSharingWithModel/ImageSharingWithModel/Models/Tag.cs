using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImageSharingWithModel.Models
{
    public class Tag
    {
        [Key]
        public virtual int Id { get; set; }
        [MaxLength(20)]
        public virtual string name { get; set; }
        public virtual ICollection<Image> Images { get; set; }
    }
}