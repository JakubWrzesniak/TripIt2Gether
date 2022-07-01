using System;
using System.ComponentModel.DataAnnotations;
using TripIt2Gether.CustomValidation;

namespace TripIt2Gether.ViewModels
{
    public class ApplicationPreviewDataViewModel
    {
        public ApplicationPreviewDataViewModel()
        {
        }

        [Key]
        public string userId { get; set; }

        [Required]
        public string TripNumber { get; set; }

        public DateTime TripStartDate { get; set; }

        [StringLength(40, MinimumLength = 1, ErrorMessage = "Name shoud contains between 1 and 40 signs")]
        [Required]
        public string Name { get; set; }

        [StringLength(50, MinimumLength = 1, ErrorMessage = "Surname shoud contains between 1 and 40 signs")]
        [Required]
        public string Surname { get; set; }

        [MaxLength(100)]
        [Required]
        [StringLength(100, MinimumLength = 10, ErrorMessage = "Address shoud contains between 10 and 100 signs")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Date of Birth")]
        [Min18Years]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        public bool CheckIfUserIsWillBeAdultOnDate(DateTime userBirthday, DateTime date)
        {
            var useBirthaDay = userBirthday.AddYears(18);
            return useBirthaDay >= date;
        }

    }
}
