using ServiceContracts.DTO;

namespace ServiceContracts
{
    public interface ICountryService
    {
        CountryResponse Add(CountryAddRequest countryAddRequest);
        IEnumerable<CountryResponse> GetAll();
        CountryResponse? GetById(Guid id);
    }
}