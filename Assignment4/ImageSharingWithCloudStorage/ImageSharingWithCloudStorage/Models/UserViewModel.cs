using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImageSharingWithCloudStorage.Models
{
    public class UserViewModel
    {
        [Required]
        [RegularExpression(@"[a-zA-Z0-9_]+", ErrorMessage = "User Id must only consist of characters, numbers or underscore")]
        public string UserId { get; set; }

        [Required]
        public bool ADA { get; set; }
    }
}