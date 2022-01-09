using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripIt2Gether.Models
{
    public class Trip
    {

        [Key]
        [Display(Name = "Trip Number")]
        [MaxLength(50)]
        public string TripNumber { get; set; }

        [Required]
        [Display(Name = "Trip name")]
        [MaxLength(40, ErrorMessage = " To long name, do not exceed {0}")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Maximum number of participants")]
        [Range(0, 100.0)]
        public int MaxNumberOfParticipants { get; set; }

        [Required]
        [Display(Name = "Maximum number of participants")]
        public TripStatus Status { get; set; }

        public string Image { get; set; }

        [ForeignKey("Form")]
        public int FormId { get; set; }

        public Form Form { get; set; }

        public ICollection<Application> Applications {get; set;}

        public ICollection<TourOperator> TourOperators {get; set;}

        public Trip()
        {
            Status = TripStatus.Projekt;
        }
    }
}
