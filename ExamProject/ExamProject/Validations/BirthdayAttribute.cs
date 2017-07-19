using System;
using System.ComponentModel.DataAnnotations;

namespace ExamProject.Validations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class BirthdayAttribute : ValidationAttribute
    {
        public BirthdayAttribute()
        {
            this.ErrorMessage = "Invalid Birthday";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationcontext)
        {
            var parsedDate = new DateTime();
            string input = value as string;

            if (DateTime.TryParse(input, out parsedDate))
            {
                if ((DateTime.Now - parsedDate).Days < 18 * 365)
                {
                    return new ValidationResult("Person must be at least 18 years old.");
                }
                else
                {
                    return ValidationResult.Success;
                }
            }
            else
            {
                return new ValidationResult("Insert a valid birthday.");
            }
        }
    }
}