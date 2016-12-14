using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ExamProject.Models
{
    public class Advertisement
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [ForeignKey("Seller")]
        public string SellerId { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        public virtual ApplicationUser Seller { get; set; }

        [Required]
        public bool IsSold { get; set; }

        [ForeignKey("AdImage")]
        public int ImageId { get; set; }

        public virtual Image AdImage { get; set; }



        
    }
}