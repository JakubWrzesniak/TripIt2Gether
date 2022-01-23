using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using TripIt2Gether.CustomValidation;

namespace TripIt2Gether.Models
{
    public class Trip
    {

        [Key]
        [Display(Name = "Trip Number")]
        [MaxLength(50)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string TripNumber { get; set; }

        [Required]
        [Display(Name = "Trip name")]
        [MaxLength(40, ErrorMessage = " To long name, do not exceed {0}")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        [StartDateBeforeEndDate]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        [Display(Name = "Maximum number of participants")]
        [Range(0, 100.0)]
        public int MaxNumberOfParticipants { get; set; }

        [Required]
        [Display(Name = "Status")]
        public TripStatus Status { get; set; } = TripStatus.Projekt;

        [Required]
        [Range(0.01, 1000000)]
        public double Cost { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }

        public string Image { get; set; }

        public IFormFile IFromImage { get; set; }

        [ForeignKey("Form")]
        public int? FormId { get; set; }

        public Form Form { get; set; }

        public ICollection<Application> Applications {get; set;}

        public ICollection<TourOperator> TourOperators {get; set;}
    }
}
