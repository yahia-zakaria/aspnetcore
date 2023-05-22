using AutoMapper;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using System.Diagnostics.Metrics;

namespace Services
{
    public class CountryService : ICountryService
    {
        readonly List<Country> _countries;
        private readonly IMapper _mapper;

        public CountryService(IMapper mapper, bool initialize = true)
        {
            _countries = new List<Country>();
            _mapper = mapper;
            if (initialize)
            {
                _countries.AddRange(
                    new List<Country>() {
                    new Country { Id = Guid.Parse("A7872C03-9643-47D1-AB56-F603F2ABA8B1"), CountryName = "USA"},
                    new Country { Id = Guid.Parse("8F1DA55F-7DFB-4CAA-9785-6F901336D6DC"), CountryName = "UK"},
                    new Country { Id = Guid.Parse("F225CCCA-10C7-44BD-886A-8D0EA28ED1C3"), CountryName = "Austrailia"},
                    new Country { Id = Guid.Parse("B3E3C9A0-0925-4493-9E24-569C89A58EAD"), CountryName = "Canada"},
                    new Country { Id = Guid.Parse("079C9AF0-BEA6-4407-B4CB-C960E8CEB4B6"), CountryName = "South Korea"}
                    }
					);
            }
        }

        public CountryResponse Add(CountryAddRequest countryAddRequest)
        {
            if(countryAddRequest == null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest));
            }
            if(countryAddRequest.CountryName == null)
            {
                throw new ArgumentException(nameof(countryAddRequest.CountryName));
            }
            if(_countries.Any(cntry=>cntry.CountryName == countryAddRequest.CountryName))
            {
                throw new ArgumentException("Duplicate country name");
            }
            var country = _mapper.Map<Country>(countryAddRequest);
            country.Id = Guid.NewGuid();
             _countries.Add(country);

            return _mapper.Map<CountryResponse>(country);
        }

        public IEnumerable<CountryResponse> GetAll()
        {
            var CountriesResponse = _countries.Select(country => _mapper.Map<CountryResponse>(country));

			return CountriesResponse;
        }

        public CountryResponse? GetById(Guid id)
        {
            return _mapper.Map<CountryResponse>(_countries.FirstOrDefault(country => country.Id == id));
        }
    }
}