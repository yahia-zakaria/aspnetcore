using AutoMapper;
using Entities;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Mapping
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<PersonAddRequest, Person>()
                .ForMember(dest=>dest.Gender, memberOptions=>memberOptions.MapFrom(src=>src.Gender.ToString()));
            CreateMap<Person, PersonResponse>();
            CreateMap<CountryAddRequest, Country>();
            CreateMap<Country, CountryResponse>();
        }
    }
}
