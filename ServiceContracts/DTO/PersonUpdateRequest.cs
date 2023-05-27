using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class PersonUpdateRequest
    {
        [Required]
        public Guid Id { get; set; }
		[Required]
		public string PersonName { get; set; }
		[EmailAddress]
		public string Email { get; set; }
		[Required]
		public DateTime? DateOfBirth { get; set; }
		[Required]
		public GenderOptions Gender { get; set; }
		[Required]
		public Guid CountryId { get; set; }
		[Required]
		public string Address { get; set; }
		public bool ReceiveNewsLetters { get; set; }
		[Required]
		public string TIN { get; set; }
	}
}
