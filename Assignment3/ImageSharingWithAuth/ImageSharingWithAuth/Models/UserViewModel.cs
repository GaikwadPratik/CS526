using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImageSharingWithAuth.Models
{
    public class UserViewModel
    {
        [Required]
        [RegularExpression(@"[a-zA-Z0-9_]+", ErrorMessage = "Please Enter Valid UserId!")]
        public string Userid { get; set; }

        [Required]
        public bool ADA { get; set; }
    }
}