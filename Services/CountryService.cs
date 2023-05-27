using AutoMapper;
using AutoMapper.QueryableExtensions;
using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using System.Diagnostics.Metrics;

namespace Services
{
    public class CountryService : ICountryService
    {
        private readonly IMapper _mapper;
		private readonly PersonsDbContext _db;

		public CountryService(PersonsDbContext db, IMapper mapper)
        {
            _mapper = mapper;
            _db = db;
        }

        public async Task<CountryResponse> Add(CountryAddRequest countryAddRequest)
        {
            if(countryAddRequest == null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest));
            }
            if(countryAddRequest.CountryName == null)
            {
                throw new ArgumentException(nameof(countryAddRequest.CountryName));
            }
            if(await _db.Countries.AnyAsync(cntry=>cntry.CountryName == countryAddRequest.CountryName))
            {
                throw new ArgumentException("Duplicate country name");
            }
            var country = _mapper.Map<Country>(countryAddRequest);
            country.Id = Guid.NewGuid();

             _db.Countries.Add(country);
			await _db.SaveChangesAsync();

			return _mapper.Map<CountryResponse>(country);
        }

        public async Task<IEnumerable<CountryResponse>> GetAll()
        {
            var CountriesResponse = await _db.Countries.ProjectTo<CountryResponse>(_mapper.ConfigurationProvider).ToListAsync();

			return CountriesResponse;
        }

        public async Task<CountryResponse?> GetById(Guid id)
        {
            return _mapper.Map<CountryResponse>(await _db.Countries.FindAsync(id));
        }
    }
}