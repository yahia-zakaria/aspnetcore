using ServiceContracts.DTO;

namespace ServiceContracts
{
    public interface ICountryService
    {
        Task<CountryResponse> Add(CountryAddRequest countryAddRequest);
        Task<IEnumerable<CountryResponse>> GetAll();
        Task<CountryResponse?> GetById(Guid id);

    }
}