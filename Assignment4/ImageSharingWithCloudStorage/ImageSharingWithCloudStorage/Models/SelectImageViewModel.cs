using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImageSharingWithCloudStorage.Models
{
    public class SelectImageViewModel
    {
        public int Id { get; set; }
        public string Caption { get; set; }
        public bool IsApproved { get; set; }
        public bool IsDeleted { get; set; }

        public SelectImageViewModel()
        {

        }

        public SelectImageViewModel(int id, string caption, bool isApproved)
        {
            Id = id;
            Caption = caption;
            IsApproved = isApproved;
        }
    }
}