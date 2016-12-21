using ExamProject.Validations;
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

       
        [DataType(DataType.ImageUrl)]
        [Display(Name ="Image")]
        public string ImageUrl { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        [Display(Name = "Available")]
        public bool IsSold { get; set; }

        
        [Display(Name = "Image Edit")]
        [DataType(DataType.Upload)]
        [Image]
        public HttpPostedFileBase ImageUpload { get; set; }

        [Display(Name = "Category Name")]
        public int CategoryId { get; set; }

        public ICollection<Category> Categories { get; set; }
    }
}