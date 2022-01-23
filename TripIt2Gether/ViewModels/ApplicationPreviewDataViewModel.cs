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

        [StringLength(40, MinimumLength = 3, ErrorMessage = " To long name, do not exceed {0}")]
        [Required]
        public string Name { get; set; }

        [StringLength(50, MinimumLength = 5, ErrorMessage = " To long Surname, do not exceed {0}")]
        [Required]
        public string Surname { get; set; }

        [MaxLength(255)]
        [Required]
        [StringLength(50, MinimumLength = 10, ErrorMessage = " To long Address, do not exceed {0}")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Date of Birth")]
        [Min18Years]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(50, MinimumLength = 5, ErrorMessage = " To long email, do not exceed {0}")]
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
