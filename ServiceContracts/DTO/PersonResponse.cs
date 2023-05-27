using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class PersonResponse
    {
        public Guid Id { get; set; }
        public string PersonName { get; set; }
        public string Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? Age
        {
            get
            {
                if (DateOfBirth is not null)
                {
                    return (int)Math.Round((DateTime.Now - DateOfBirth.Value).TotalDays / 365.25);
                }
                else
                {
                    return null;
                }
            }
        }
        public string Gender { get; set; }
        public Guid CountryId { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public bool ReceiveNewsLetters { get; set; }
        public string TIN { get; set; }

		public override bool Equals(object obj)
        {
            if(obj == null || !(obj is PersonResponse)) return false;
            PersonResponse other = (PersonResponse)obj;

            return Id == other.Id && PersonName == other.PersonName && Email == other.Email && DateOfBirth == other.DateOfBirth 
                &&  Age == other.Age && CountryId == other.CountryId && Address == other.Address && ReceiveNewsLetters == other.ReceiveNewsLetters
                && TIN == other.TIN; 
        }

        public override string ToString()
        {
            return $"PersonId: {Id}, PersonName: {PersonName}, Email: {Email}, Date of Birth: {DateOfBirth}," +
                $" Age: {Age}, Country: {Country}, Address: {Address}, Receive News Letters: {ReceiveNewsLetters}, TaxIdentificationNumber: {TIN}";
        }
    }
}
