using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Person
    {
        public Guid Id { get; set; }
        [Required]
        public string PersonName { get; set; }
        [EmailAddress]
        public string Email { get; set; } 
        public DateTime? DateOfBirth { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public Guid CountryId { get; set; }
        [Required]
        public string Address { get; set; }
        public bool ReceiveNewsLetters { get; set; } = false;
		public string TIN { get; set; }
		public Country Country { get; set; }
	}
}
