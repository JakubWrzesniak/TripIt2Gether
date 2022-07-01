using System;
using System.ComponentModel.DataAnnotations;
using TripIt2Gether.Models;
using TripIt2Gether.Areas.Identity.Pages.Account.Manage;

namespace TripIt2Gether.CustomValidation
{
    public class DateBeforeToday : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(validationContext is ApplicationUser)
            {
                var user = (ApplicationUser)validationContext.ObjectInstance;

                if (user.DateOfBirth == null)
                    return new ValidationResult("Date of Birth is required.");

                return user.DateOfBirth < DateTime.Now ? ValidationResult.Success : new ValidationResult("You Birthday Must be before Current date");

            }
            else { 
            
                var user = (TripIt2Gether.Areas.Identity.Pages.Account.Manage.IndexModel.InputModel)validationContext.ObjectInstance;

                if (user.DateOfBirth == null)
                    return new ValidationResult("Date of Birth is required.");

                return user.DateOfBirth < DateTime.Now ? ValidationResult.Success : new ValidationResult("You Birthday Must be before Current date");
            }
        }
    }
}
