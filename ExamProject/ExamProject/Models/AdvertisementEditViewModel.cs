using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExamProject.Models
{
    public class AdvertisementEditViewModel
    {
        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        [Display(Name ="Image")]
        public string ImageUrl { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        [Display(Name = "Available")]
        public bool IsSold { get; set; }

        [Required]
        [Display(Name = "Image Edit")]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase ImageUpload { get; set; }
    }
}