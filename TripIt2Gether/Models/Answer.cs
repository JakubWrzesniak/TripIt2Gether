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
        [StringLength(455, MinimumLength = 5, ErrorMessage = "Odpowiediź powinna zawierać od 5 do 455 znaków")]
        public string Content { get; set; }

        public Question Question { get; set; }

        public Application Application { get; set; }
    }
}
