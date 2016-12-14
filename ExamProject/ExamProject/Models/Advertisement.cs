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
        public Advertisement()
        {
            this.IsSold = false;
            this.CreationDate = DateTime.Now;
        }

        public Advertisement(string sellerId,string title, string description, double price)
        {
            this.SellerId = sellerId;
            this.Title = title;
            this.Description = description;
            this.Price = System.Math.Round(price, 2);
            this.IsSold = false;
            this.CreationDate = DateTime.Now;
        }

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

        [Required]
        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }

        [Required]
        public double Price { get; set; }



        
    }
}