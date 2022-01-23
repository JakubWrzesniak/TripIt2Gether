using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using TripIt2Gether.ViewModels;

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

        [Required]
        public bool IsPaid { get; set; } = false;

        [Required]
        [Range(0.01, 100000)]
        public double Price { get; set; }

        [Required]
        public DateTime AddedDate { get; set; }

        [Required]
        public ParticipationStatus Status { get; set; } = ParticipationStatus.OczekujacaPotiwerdzenia;


        public ICollection<Answer> Answers { get; set; } = new List<Answer>();

        public Trip Trip { get; set; }

        public ApplicationUser Participant { get; set; }

        public Application()
        {
            Status = ParticipationStatus.OczekujacaPotiwerdzenia;
        }

        public bool CheckIfUserAnsweredForAllQuestions()
        {
            return Answers.Count == Trip.Form.Questions.Count;
        }
    }
}
