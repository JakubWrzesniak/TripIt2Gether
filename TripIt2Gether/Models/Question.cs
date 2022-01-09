using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TripIt2Gether.Models
{
    public class Question
    {
        [Key]
        public int QuestionId { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        [MaxLength(255, ErrorMessage = "Content is too long")]
        public int FormId { get; set;}

        [Required]
        public Form Form { get; set; }

        public ICollection<Answer> Answers { get; set; }

        public Question()
        {
        }
    }
}
