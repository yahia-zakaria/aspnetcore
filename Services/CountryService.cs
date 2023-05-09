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
        private readonly IMapper mapper;

        public CountryService(IMapper mapper)
        {
            _countries = new();
            this.mapper = mapper;
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
            var country = mapper.Map<Country>(countryAddRequest);
            country.Id = Guid.NewGuid();
             _countries.Add(country);

            return mapper.Map<CountryResponse>(country);
        }

        public IEnumerable<CountryResponse> GetAll()
        {
            return _countries.Select(country => mapper.Map<CountryResponse>(country));
        }

        public CountryResponse? GetById(Guid id)
        {
            return mapper.Map<CountryResponse>(_countries.FirstOrDefault(country => country.Id == id));
        }
    }
}