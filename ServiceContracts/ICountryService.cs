using Microsoft.AspNetCore.Http;
using ServiceContracts.DTO;

namespace ServiceContracts
{
    public interface ICountryService
    {
        Task<CountryResponse> Add(CountryAddRequest countryAddRequest);
        Task<List<CountryResponse>> GetAll();
        Task<CountryResponse?> GetById(Guid id);
        Task<int> UploadCountriesFromExcelFile(IFormFile formFile);


	}
}