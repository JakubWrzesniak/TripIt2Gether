using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace TripIt2Gether.Models
{
    public class TourOperator
    {
        [Key]
        public string OperatorId { get; set; }

        [Key]
        public string TripId { get; set; }

        public Trip Trip { get; set; }

        public ApplicationUser Operator { get; set; }

        public TourOperator()
        {
        }
    }
}
