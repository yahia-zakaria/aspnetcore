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
        [Required]
        public string Email { get; set; }
        [Required]
        public bool ReceiveNewsLetters { get; set; }
    }
}
