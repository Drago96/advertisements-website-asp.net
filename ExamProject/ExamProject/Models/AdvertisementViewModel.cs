using ExamProject.Validations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace ExamProject.Models
{
    public class AdvertisementViewModel
    {
        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Image Upload")]
        [DataType(DataType.Upload)]
        [Image]
        public HttpPostedFileBase ImageUpload { get; set; }

        [Required]
        public double Price { get; set; }

        [Display(Name = "Category Name")]
        public int CategoryId { get; set; }

        public ICollection<Category> Categories { get; set; }
    }
}