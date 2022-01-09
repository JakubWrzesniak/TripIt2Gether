using System;
using System.ComponentModel.DataAnnotations;

namespace TripIt2Gether.Models
{
    public class Answer { 

        [Key]
        public string ParticipantId { get; set; }

        [Key]
        public int QuestionId { get; set; }

        [Key]
        public string TripNumber { get; set; }

        [Required]
        [MaxLength(450)]
        public string Content { get; set; }

        public Question Question { get; set; }

        public Application Application { get; set; }
    }
}
