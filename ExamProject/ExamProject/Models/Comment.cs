using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ExamProject.Models
{
    public class Comment
    {
        public Comment()
        {
            
            this.CreatedOn = DateTime.Now;
        }

        public Comment(string content, string authorId, string targetId)
        {
            
            this.Content = content;
            this.AuthorId = authorId;
            this.TargetId = targetId;
            this.CreatedOn = DateTime.Now;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [ForeignKey("Author")]
        public string AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }
        

        [ForeignKey("Target")]
        public string TargetId { get; set; }

        public virtual ApplicationUser Target { get; set; }

        public bool IsAuthor(string name)
        {
            return this.Author.UserName.Equals(name);
        }

    }
}