using System.ComponentModel.DataAnnotations;

namespace ExamProject.Models
{
    public class CommentEditViewModel
    {
        [Required]
        public string Content { get; set; }

        [Required]
        public string TargetName { get; set; }
    }
}