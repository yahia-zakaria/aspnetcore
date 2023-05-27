using AutoMapper;
using AutoMapper.QueryableExtensions;
using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
	public class PersonService : IPersonService
	{
		private readonly ICountryService _countryService;
		private readonly PersonsDbContext _db;
		private readonly IMapper _mapper;
		public PersonService(PersonsDbContext db, IMapper mapper, ICountryService countryService)
		{
			_db = db;
			_mapper = mapper;
			_countryService = countryService;
		}

		public async Task<PersonResponse> Add(PersonAddRequest person)
		{
			if (person == null) throw new ArgumentNullException(nameof(person));
			if (person.PersonName is null) throw new ArgumentException(nameof(person.PersonName));

			var personToAdd = _mapper.Map<Person>(person);
			personToAdd.Id = Guid.NewGuid();
			_db.Persons.Add(personToAdd);
			await _db.SaveChangesAsync();

			var personResponse = _mapper.Map<PersonResponse>(personToAdd);
			return personResponse;
		}

		public async Task<bool> Delete(Guid id)
		{
			if (id == Guid.Empty)
				return false;

			var person = await _db.Persons.FindAsync(id);

			if (person is null)
				return false;

			_db.Persons.Remove(person);
			await _db.SaveChangesAsync();

			return true;
		}

		public async Task<List<PersonResponse>> GetAll()
		{
			var allPersons = await _db.Persons.Include(i=>i.Country).ProjectTo<PersonResponse>(_mapper.ConfigurationProvider).ToListAsync();
			return allPersons;
		}


		public async Task<PersonResponse> GetById(Guid id)
		{
			if(id == Guid.Empty)
				throw new ArgumentNullException("id");

			var person = _mapper.Map<PersonResponse>(await _db.Persons.Include(i=>i.Country).FirstOrDefaultAsync(person=>person.Id==id));
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
			if (request is null)
				throw new ArgumentNullException();

			var person = await _db.Persons.FindAsync(request.Id);
			if (person == null)
				throw new ArgumentException("The given ID doesn't exist");

			ValidationHelper.Validate(request);

			person = _mapper.Map(request, person);
			await _db.SaveChangesAsync();

			return _mapper.Map<PersonResponse>(person);


		}

	}
}
