using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class CountryResponse
    {
        public Guid Id { get; set; }
        public string? CountryName { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if(GetType() != obj.GetType()) return false;
            CountryResponse other = obj as CountryResponse;

            return Id == other.Id && CountryName == other.CountryName;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }

}
