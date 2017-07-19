using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExamProject.Validations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ImageAttribute : ValidationAttribute
    {
        public ImageAttribute()
        {
            this.ErrorMessage = "File is not an image.";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationcontext)
        {
            var input = value as HttpPostedFileBase;

            var validImageTypes = new string[]
             {
                 "image/gif",
                 "image/jpeg",
                 "image/pjpeg",
                 "image/png"
             };

            if (input != null)
            {
                if (!validImageTypes.Contains(input.ContentType))
                {
                    return new ValidationResult("File is not a valid image type");
                }
                else
                {
                    return ValidationResult.Success;
                }
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}