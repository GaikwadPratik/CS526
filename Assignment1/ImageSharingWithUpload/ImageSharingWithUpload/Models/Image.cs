using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImageSharingWithUpload.Models
{
    public class Image
    {
        /// <summary>
        /// Id of the image
        /// </summary>
        [Required]
        [RegularExpression(@"[a-zA-Z0-9_]+")]
        [Display(Name = "Image Id")]
        public string Id { get; set; }

        /// <summary>
        /// Caption for the image in upto 40 characters
        /// </summary>
        [Required]
        [MaxLength(40, ErrorMessage = "Max length for caption exceeded")]
        public string Caption { get; set; }

        /// <summary>
        /// Description of the image in upto 200 characters
        /// </summary>
        [Required]
        [MaxLength(200, ErrorMessage = "Max length for description exceeded")]
        public string Description { get; set; }

        /// <summary>
        /// Date on which image was captured
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date Taken")]
        public DateTime DateTaken { get; set; }

        /// <summary>
        /// Id of the user for which the image is being uploaded
        /// </summary>
        public string UserId { get; set; }
    }
}