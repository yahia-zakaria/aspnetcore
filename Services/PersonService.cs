using AutoMapper;
using AutoMapper.QueryableExtensions;
using CsvHelper;
using Entities;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using ServiceContracts.Repository;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
	public class PersonService : IPersonService
	{
		private readonly ICountryService _countryService;
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;
        public PersonService(IUnitOfWork unitOfWork, IMapper mapper, ICountryService countryService)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_countryService = countryService;
		}

		public async Task<PersonResponse> Add(PersonAddRequest person)
		{
			var personRepository = _unitOfWork.GetRepository<Person>();

            if (person == null) throw new ArgumentNullException(nameof(person));
			if (person.PersonName is null) throw new ArgumentException(nameof(person.PersonName));

			var personToAdd = _mapper.Map<Person>(person);
			personToAdd.Id = Guid.NewGuid();
            personRepository.Add(personToAdd);
			await _unitOfWork.SaveChangesAsync();

			var personResponse = _mapper.Map<PersonResponse>(personToAdd);
			return personResponse;
		}

		public async Task<bool> Delete(Guid id)
		{
            var personRepository = _unitOfWork.GetRepository<Person>();
            if (id == Guid.Empty)
				return false;

			var person = await personRepository.GetByIdAsync(id);

			if (person is null)
				return false;

			personRepository.Remove(person);
			await _unitOfWork.SaveChangesAsync();

			return true;
		}

		public async Task<List<PersonResponse>> GetAll()
		{
            var personRepository = _unitOfWork.GetRepository<Person>();
            var allPersons = await personRepository.GetAllAsync().Include(i=>i.Country).ProjectTo<PersonResponse>(_mapper.ConfigurationProvider).ToListAsync();
			return allPersons;
		}


		public async Task<PersonResponse> GetById(Guid id)
		{
            var personRepository = _unitOfWork.GetRepository<Person>();
            if (id == Guid.Empty)
				throw new ArgumentNullException("id");

			var person = _mapper.Map<PersonResponse>(await personRepository.GetAllAsync().Include(i=>i.Country).FirstOrDefaultAsync(person=>person.Id==id));
			return person;
		}

		public async Task<List<PersonResponse>> GetFilteredPerson(string searchBy, string SearchString)
		{
			var allPersons = await GetAll();
			var matchingPersons = allPersons;

			if (string.IsNullOrWhiteSpace(searchBy))
				return matchingPersons;

			switch (searchBy)
			{
				case nameof(PersonResponse.PersonName):
					matchingPersons = SearchString != null ?
					 allPersons.Where(person => person.PersonName.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
					   .Select(p => _mapper.Map<PersonResponse>(p))
					   .ToList() : allPersons;
					break;

				case nameof(PersonResponse.Email):
					matchingPersons = SearchString != null ?
					 allPersons.Where(person => person.Email.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
					   .Select(p => _mapper.Map<PersonResponse>(p))
					   .ToList() : allPersons;
					break;
				case nameof(PersonResponse.Age):
					matchingPersons = SearchString != null ?
					 allPersons.Where(person => int.TryParse(SearchString, out int age) ? person.Age == age : person.Age == 0)
					   .Select(p => _mapper.Map<PersonResponse>(p))
					   .ToList() : allPersons;
					break;

				case nameof(PersonResponse.DateOfBirth):
					matchingPersons = SearchString != null ?
					allPersons.Where(person => (person.DateOfBirth != null) ?
					 person.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(SearchString) : true)
					   .Select(p => _mapper.Map<PersonResponse>(p))
					   .ToList() : allPersons;
					break;

				case nameof(PersonResponse.Gender):
					matchingPersons = SearchString != null ?
					 allPersons.Where(person => person.Gender.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
					   .Select(p => _mapper.Map<PersonResponse>(p))
					   .ToList() : allPersons;
					break;


				case nameof(PersonResponse.Address):
					matchingPersons = SearchString != null ?
					 allPersons.Where(person => person.Address.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
					   .Select(p => _mapper.Map<PersonResponse>(p))
					   .ToList() : allPersons;
					break;

				case nameof(PersonResponse.TIN):
					matchingPersons = SearchString != null ?
					 allPersons.Where(person => person.TIN.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
					   .Select(p => _mapper.Map<PersonResponse>(p))
					   .ToList() : allPersons;
					break;

				default:
					matchingPersons = allPersons; break;
			}

			return matchingPersons;
		}

		public List<PersonResponse> GetSortedPersons(List<PersonResponse> persons, string sortBy, SortOptions sortDir)
		{
			var allPerson = persons;

			if (sortBy is null)
				return allPerson;

			var sortedPersons = new List<PersonResponse>();
			sortedPersons = (sortBy, sortDir)
				switch
			{
				(nameof(PersonResponse.PersonName), SortOptions.ASCENDING) =>
				allPerson.OrderBy(person => person.PersonName).ToList(),
				(nameof(PersonResponse.PersonName), SortOptions.DESCENDING) =>
				allPerson.OrderByDescending(person => person.PersonName).ToList(),

				(nameof(PersonResponse.Email), SortOptions.ASCENDING) =>
				allPerson.OrderBy(person => person.Email).ToList(),
				(nameof(PersonResponse.Email), SortOptions.DESCENDING) =>
				allPerson.OrderByDescending(person => person.Email).ToList(),

				(nameof(PersonResponse.DateOfBirth), SortOptions.ASCENDING) =>
				allPerson.OrderBy(person => person.DateOfBirth).ToList(),
				(nameof(PersonResponse.DateOfBirth), SortOptions.DESCENDING) =>
				allPerson.OrderByDescending(person => person.DateOfBirth).ToList(),

				(nameof(PersonResponse.Age), SortOptions.ASCENDING) =>
				allPerson.OrderBy(person => person.Age).ToList(),
				(nameof(PersonResponse.Age), SortOptions.DESCENDING) =>
				allPerson.OrderByDescending(person => person.Age).ToList(),

				(nameof(PersonResponse.Gender), SortOptions.ASCENDING) =>
				allPerson.OrderBy(person => person.Gender).ToList(),
				(nameof(PersonResponse.Gender), SortOptions.DESCENDING) =>
				allPerson.OrderByDescending(person => person.Gender).ToList(),

				(nameof(PersonResponse.Country), SortOptions.ASCENDING) =>
				allPerson.OrderBy(person => person.Country).ToList(),
				(nameof(PersonResponse.Country), SortOptions.DESCENDING) =>
				allPerson.OrderByDescending(person => person.Country).ToList(),

				(nameof(PersonResponse.Address), SortOptions.ASCENDING) =>
				allPerson.OrderBy(person => person.Address).ToList(),
				(nameof(PersonResponse.Address), SortOptions.DESCENDING) =>
				allPerson.OrderByDescending(person => person.Address).ToList(),

				(nameof(PersonResponse.ReceiveNewsLetters), SortOptions.ASCENDING) =>
				allPerson.OrderBy(person => person.ReceiveNewsLetters).ToList(),
				(nameof(PersonResponse.ReceiveNewsLetters), SortOptions.DESCENDING) =>
				allPerson.OrderByDescending(person => person.ReceiveNewsLetters).ToList(),

				(nameof(PersonResponse.TIN), SortOptions.ASCENDING) =>
				allPerson.OrderBy(person => person.TIN).ToList(),
				(nameof(PersonResponse.TIN), SortOptions.DESCENDING) =>
				allPerson.OrderByDescending(person => person.TIN).ToList(),

			};

			return sortedPersons;
		}

		public async Task<PersonResponse> Update(PersonUpdateRequest request)
		{
            var personRepository = _unitOfWork.GetRepository<Person>();
            if (request is null)
				throw new ArgumentNullException();

			var person = await personRepository.GetByIdAsync(request.Id);
			if (person == null)
				throw new ArgumentException("The given ID doesn't exist");

			ValidationHelper.Validate(request);

			person = _mapper.Map(request, person);
			await _unitOfWork.SaveChangesAsync();

			return _mapper.Map<PersonResponse>(person);


		}


		public async Task<MemoryStream> GetPersonsCSV()
		{
            var personRepository = _unitOfWork.GetRepository<Person>();
            MemoryStream memoryStream = new MemoryStream();
			StreamWriter streamWriter = new StreamWriter(memoryStream);
			CsvWriter csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture, leaveOpen: true);

			csvWriter.WriteHeader<PersonResponse>(); //PersonID,PersonName,...
			csvWriter.NextRecord();

			List<PersonResponse> persons = personRepository.GetAllAsync()
			  .Include("Country")
			  .ProjectTo<PersonResponse>(_mapper.ConfigurationProvider).ToList();

			await csvWriter.WriteRecordsAsync(persons);
			//1,abc,....

			memoryStream.Position = 0;
			return memoryStream;
		}

		public async Task<MemoryStream> GetPersonsExcel()
		{
            var personRepository = _unitOfWork.GetRepository<Person>();
            MemoryStream memoryStream = new MemoryStream();
			using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
			{
				ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets.Add("PersonsSheet");
				workSheet.Cells["A1"].Value = "Person Name";
				workSheet.Cells["B1"].Value = "Email";
				workSheet.Cells["C1"].Value = "Date of Birth";
				workSheet.Cells["D1"].Value = "Age";
				workSheet.Cells["E1"].Value = "Gender";
				workSheet.Cells["F1"].Value = "Country";
				workSheet.Cells["G1"].Value = "Address";
				workSheet.Cells["H1"].Value = "Receive News Letters";

				using (ExcelRange headerCells = workSheet.Cells["A1:H1"])
				{
					headerCells.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					headerCells.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
					headerCells.Style.Font.Bold = true;
				}

				int row = 2;

				List<PersonResponse> persons = personRepository.GetAllAsync()
						  .Include("Country")
						  .ProjectTo<PersonResponse>(_mapper.ConfigurationProvider).ToList();

				foreach (PersonResponse person in persons)
				{
					workSheet.Cells[row, 1].Value = person.PersonName;
					workSheet.Cells[row, 2].Value = person.Email;
					if (person.DateOfBirth.HasValue)
						workSheet.Cells[row, 3].Value = person.DateOfBirth.Value.ToString("yyyy-MM-dd");
					workSheet.Cells[row, 4].Value = person.Age;
					workSheet.Cells[row, 5].Value = person.Gender;
					workSheet.Cells[row, 6].Value = person.Country;
					workSheet.Cells[row, 7].Value = person.Address;
					workSheet.Cells[row, 8].Value = person.ReceiveNewsLetters;

					row++;
				}

				workSheet.Cells[$"A1:H{row}"].AutoFitColumns();

				await excelPackage.SaveAsync();
			}

			memoryStream.Position = 0;
			return memoryStream;
		}
	}
}
