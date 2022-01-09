using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripIt2Gether.Models
{
    public class Form
    {
        [Key]
        public int FormId { get; set; }

        public DateTime aktywnyDo { get; set; }

        [Required]
        [Display(Name = "Category")]
        [ForeignKey("Trip")]
        public string TripId { get; set; }

        public Trip Trip { get; set; }

        public ICollection<Question> Questions { get; set; }
    }
}
