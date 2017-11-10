using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImageSharingWithCloudStorage.Models
{
    public class ImageViewModel
    {
        /// <summary>
        /// Tag id for the image
        /// </summary>
        [Required]
        public int TagId { get; set; }

        /// <summary>
        /// Caption for the image in upto 40 characters
        /// </summary>
        [Required]
        [StringLength(40, ErrorMessage = "Max length for caption exceeded")]
        public string Caption { get; set; }

        /// <summary>
        /// Description of the image in upto 200 characters
        /// </summary>
        [Required]
        [StringLength(200, ErrorMessage = "Max length for description exceeded")]
        public string Description { get; set; }

        /// <summary>
        /// Date on which image was captured
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date Taken")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime DateTaken { get; set; }

        [ScaffoldColumn(false)]
        public int Id { get; internal set; }
		[ScaffoldColumn(false)]
        public string Uri;

        [ScaffoldColumn(false)]
        public String TagName { get; set; }
        
        /// <summary>
        /// Id of the user for which the image is being uploaded
        /// </summary>
        [ScaffoldColumn(false)]
        public string UserId { get; set; }
    }
}