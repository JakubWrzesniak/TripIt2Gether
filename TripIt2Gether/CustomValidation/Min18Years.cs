using System;
using System.ComponentModel.DataAnnotations;
using TripIt2Gether.ViewModels;

namespace TripIt2Gether.CustomValidation
{
    public class Min18Years: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var application = (ApplicationPreviewDataViewModel)validationContext.ObjectInstance;

            if (application.DateOfBirth == null)
                return new ValidationResult("Date of Birth is required.");
            var userBirthday = application.DateOfBirth.AddYears(18);

            return (userBirthday- application.TripStartDate).Days >= 0 ? ValidationResult.Success : new ValidationResult("Have to at leats 18 Yers on trip start day");
        }
    }
}
