using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExamProject.Models
{
    public class CommentEditViewModel
    {
        [Required]
        public string Content { get; set; }
    }
}