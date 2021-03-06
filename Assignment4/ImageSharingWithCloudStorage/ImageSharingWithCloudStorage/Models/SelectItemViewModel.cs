﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImageSharingWithCloudStorage.Models
{
    public class SelectItemViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; }
        public bool Checked { get; set; }

        public SelectItemViewModel()
        {

        }

        public SelectItemViewModel(string id, string userName, bool active)
        {
            Id = id;
            UserName = userName;
            IsActive = active;
        }
    }
}