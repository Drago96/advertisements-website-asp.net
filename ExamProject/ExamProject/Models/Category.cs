using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ExamProject.Models
{
    public class Category
    {
        private ICollection<Advertisement> advertisements;

        public Category()
        {
            this.advertisements = new HashSet<Advertisement>();
        }

        [Key]
        public int Id {get;set;}

        [Required]
        [Index(IsUnique=true)]
        [StringLength(20)]
        public string Name { get; set; }

        public virtual ICollection<Advertisement> Advertisements
        {
            get { return this.advertisements; }
            set { this.advertisements = value; }
        }

    }
}