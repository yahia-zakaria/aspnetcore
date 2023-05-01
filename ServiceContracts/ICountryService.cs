using ServiceContracts.DTO;

namespace ServiceContracts
{
    public interface ICountryService
    {
        CountryAddResponse Add(CountryAddRequest countryAddRequest);
    }
}