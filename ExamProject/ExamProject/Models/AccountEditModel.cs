using ExamProject.Validations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExamProject.Models
{
    public class AccountEditModel
    {

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [StringLength(12)]
        [Display(Name = "Phone Number")]
        [RegularExpression("0([0-9]{9})", ErrorMessage = "Invalid phone number.")]
        public string PhoneNumber { get; set; }

        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Compare("Password",ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Required]
        [Birthday]
        [Display(Name = "Birth date")]
        public string Birthday { get; set; }

        public IList<Role> Roles { get; set; }
    }
}