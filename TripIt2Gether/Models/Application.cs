using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace TripIt2Gether.Models
{
    public class Application
    {
        [Key]
        public string TripId { get; set; }

        [Key]
        public string UserId { get; set; }

        [MaxLength(455)]
        public string AdditionInformation { get; set; }

        public bool IsPaid { get; set; }

        [Required]
        [Range(0.01, 100000)]
        public double Price { get; set; }

        [Required]
        public DateTime AddedDate { get; set; }

        public ParticipationStatus Status { get; set; }

        public ICollection<Answer> Answers { get; set; }

        public Trip Trip { get; set; }

        public ApplicationUser Participant { get; set; }

        public Application()
        {
            Status = ParticipationStatus.OczekujacaPotiwerdzenia;
        }
    }
}
