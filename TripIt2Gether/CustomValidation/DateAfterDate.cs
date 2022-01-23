using System;
using System.ComponentModel.DataAnnotations;
using TripIt2Gether.Models;

namespace TripIt2Gether.CustomValidation
{
    public class StartDateBeforeEndDate: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var trip = (Trip)validationContext.ObjectInstance;

            if (trip.StartDate == null)
                return new ValidationResult("Start Date is required");
            if (trip.EndDate == null)
                return new ValidationResult("End Date is required");

            return trip.EndDate > trip.StartDate ? ValidationResult.Success : new ValidationResult("End date Date must be after Start date");
        }
    }
}
