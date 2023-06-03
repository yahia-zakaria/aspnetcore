using AutoMapper;
using AutoMapper.QueryableExtensions;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Repository;

namespace Services
{
    public class CountryService : ICountryService
    {
        private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;

		public CountryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<CountryResponse> Add(CountryAddRequest countryAddRequest)
        {
            var countriesRepository = _unitOfWork.GetRepository<Country>();
            if (countryAddRequest == null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest));
            }
            if(countryAddRequest.CountryName == null)
            {
                throw new ArgumentException(nameof(countryAddRequest.CountryName));
            }
            if(await countriesRepository.AnyAsync(cntry=>cntry.CountryName == countryAddRequest.CountryName))
            {
                throw new ArgumentException("Duplicate country name");
            }
            var country = _mapper.Map<Country>(countryAddRequest);
            country.Id = Guid.NewGuid();

            country = countriesRepository.Add(country);
			await _unitOfWork.SaveChangesAsync();

			return _mapper.Map<CountryResponse>(country);
        }

        public async Task<List<CountryResponse>> GetAll()
        {
            var countriesRepository = _unitOfWork.GetRepository<Country>();
            var CountriesResponse = await countriesRepository.GetAllAsync().ProjectTo<CountryResponse>
                (_mapper.ConfigurationProvider).ToListAsync();

			return CountriesResponse;
        }

        public async Task<CountryResponse?> GetById(Guid id)
        {
            var countriesRepository = _unitOfWork.GetRepository<Country>();
            return _mapper.Map<CountryResponse>(await countriesRepository.GetByIdAsync(id));
        }

		public async Task<int> UploadCountriesFromExcelFile(IFormFile formFile)
		{
            var countriesRepository = _unitOfWork.GetRepository<Country>();
            MemoryStream memoryStream = new MemoryStream();
			await formFile.CopyToAsync(memoryStream);
			int countriesInserted = 0;

			using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
			{
				ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets[0];

				int rowCount = workSheet.Dimension.Rows;

				for (int row = 2; row <= rowCount; row++)
				{
					string? cellValue = Convert.ToString(workSheet.Cells[row, 1].Value);

					if (!string.IsNullOrEmpty(cellValue))
					{
						string? countryName = cellValue;

						if (!await countriesRepository.AnyAsync(temp => temp.CountryName == countryName))
						{
							Country country = new Country() { CountryName = countryName };
							countriesRepository.Add(country);
							await _unitOfWork.SaveChangesAsync();

							countriesInserted++;
						}
					}
				}
			}

			return countriesInserted;
		}
	}
}