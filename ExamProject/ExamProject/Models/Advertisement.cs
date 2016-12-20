using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
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

        public Advertisement(string sellerId,string title, string description, double price, int categoryId)
        {
            this.SellerId = sellerId;
            this.Title = title;
            this.Description = description;
            this.Price = System.Math.Round(price, 2);
            this.IsSold = false;
            this.CreationDate = DateTime.Now;
            this.CategoryId = categoryId;
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
        [Display(Name ="Available")]
        public bool IsSold { get; set; }
       
        [DataType(DataType.ImageUrl)]
        [Display(Name ="Image")]
        public string ImageUrl { get; set; }

        [Required]
        public double Price { get; set; }

        [ForeignKey("Category")]
        [Display(Name ="Category Name")]
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public string GetSummary()
        {
            if(this.Description.Length<100)
            {
                return this.Description;
            }
            else
            {
                return this.Description.Substring(0,100) + "...";
            }
        }

        public bool IsSeller(string name)
        {
            return this.Seller.UserName.Equals(name);
        }

       

        
    }
}