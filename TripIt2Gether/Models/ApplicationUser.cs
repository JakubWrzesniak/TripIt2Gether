using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace TripIt2Gether.Models
{
    public class ApplicationUser : IdentityUser
    {

        public DateTime DateOfBirth { get; set; }

        [MaxLength(40, ErrorMessage = " To long name, do not exceed {0}")]
        public string Name { get; set; }

        [MaxLength(50, ErrorMessage = " To long surname, do not exceed {0}")]
        public string Surname { get; set; }

        [MaxLength(255)]
        public string Address { get; set; }

        public bool IsVerified { get; set; }

        public ICollection<Application> Applications {get; set;}

        public ICollection<TourOperator> TourOperators { get; set;}

        [MaxLength(255)]
        public override string Id { get => base.Id; set => base.Id = value; }
    }
}
