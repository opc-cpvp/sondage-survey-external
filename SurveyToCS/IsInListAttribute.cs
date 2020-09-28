using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyToCS
{
    public class IsInListAttribute : ValidationAttribute
    {
        public string[] List { get; set; }

        public IsInListAttribute(string[] list, string errorMessage = "")
        {
            ErrorMessage = errorMessage;
            List = list;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!List.Contains(value))
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
