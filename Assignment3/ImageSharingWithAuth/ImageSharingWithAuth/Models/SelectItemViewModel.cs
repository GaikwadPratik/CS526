using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImageSharingWithAuth.Models
{
    public class SelectItemViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; }
        public bool Checked { get; set; }
        public SelectItemViewModel(string id = "", string userName="", bool active =false)
        {
            if (!string.IsNullOrEmpty(id))
                Id = id;
            if (!string.IsNullOrEmpty(userName))
                UserName = userName;
            IsActive = active;
        }
    }
}