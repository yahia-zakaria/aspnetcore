using AutoMapper;
using Entities;
using ServiceContracts.DTO;

namespace Services.Mapping
{
	public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<PersonAddRequest, Person>()
                .ForMember(dest=>dest.Gender, memberOptions=>memberOptions.MapFrom(src=>src.Gender.ToString()));
            CreateMap<Person, PersonResponse>()
                .ForMember(dest=>dest.Country, memberOptions=>memberOptions.MapFrom(src=>src.Country == null? string.Empty : src.Country.CountryName));
            CreateMap<PersonResponse, PersonUpdateRequest>().ReverseMap();
            CreateMap<Person, PersonUpdateRequest>().ReverseMap();
			CreateMap<CountryAddRequest, Country>();
            CreateMap<Country, CountryResponse>();
        }
    }
}
