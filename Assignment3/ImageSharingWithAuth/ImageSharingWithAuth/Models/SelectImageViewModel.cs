using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImageSharingWithAuth.Models
{
    public class SelectImageViewModel
    {
        public int Id { get; set; }
        public string Caption { get; set; }
        public bool IsApproved { get; set; }
        public bool IsDeleted { get; set; }

        public SelectImageViewModel(int id = -1, string caption = "", bool isApproved = false)
        {
            if (id != -1)
                Id = id;
            if (!string.IsNullOrEmpty(caption))
                Caption = caption;
            IsApproved = isApproved;
        }
    }
}