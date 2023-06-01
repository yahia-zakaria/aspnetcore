using AutoMapper;
using AutoMapper.QueryableExtensions;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using ServiceContracts;
using ServiceContracts.DTO;
using System.Diagnostics.Metrics;

namespace Services
{
    public class CountryService : ICountryService
    {
        private readonly IMapper _mapper;
		private readonly ApplicationDbContext _db;

		public CountryService(ApplicationDbContext db, IMapper mapper)
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

		public async Task<int> UploadCountriesFromExcelFile(IFormFile formFile)
		{
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

						if (_db.Countries.Where(temp => temp.CountryName == countryName).Count() == 0)
						{
							Country country = new Country() { CountryName = countryName };
							_db.Countries.Add(country);
							await _db.SaveChangesAsync();

							countriesInserted++;
						}
					}
				}
			}

			return countriesInserted;
		}
	}
}