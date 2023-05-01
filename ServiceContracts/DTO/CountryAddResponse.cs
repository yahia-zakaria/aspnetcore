using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class CountryAddResponse
    {
        public Guid CountryId { get; set; }
        public string? CountryName { get; set; }
    }

    public static class CountryExtensions
    {
        public static CountryAddResponse ToCountryResponse(this Country country)
        {
            return new CountryAddResponse { CountryId = country.CountryId, CountryName = country.CountryName };
        }
    }
}
