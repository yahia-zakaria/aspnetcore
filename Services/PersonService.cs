using AutoMapper;
using Entities;
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
		private readonly List<Person> _persons;
		private readonly ICountryService _countryService;
		private readonly IMapper _mapper;
		public PersonService(IMapper mapper, bool initialize = true)
		{
			_persons = new();
			_mapper = mapper;
			_countryService = new CountryService(mapper, true);
			if (initialize)
			{
				_persons.AddRange(
					new List<Person>
					{
						new Person
						{
							Id = Guid.Parse("6D875B26-7ED8-4353-AC1D-B61091F47FA9"),
							PersonName = "Yahia Zakaria",
							Email = "a@a.com",
							Address = "UK Liverpool",
							CountryId = Guid.Parse("8F1DA55F-7DFB-4CAA-9785-6F901336D6DC"),
							DateOfBirth = DateTime.Parse("1995/06/10"),
							Gender = GenderOptions.Male.ToString(),
							ReceiveNewsLetters = true
						},
						new Person
						{
							Id = Guid.Parse("337E4125-3830-4E62-91B0-CABF0752B3D4"),
							PersonName = "Ali Osman",
							Email = "b@b.com",
							Address = "USA, Washitone DC",
							CountryId = Guid.Parse("A7872C03-9643-47D1-AB56-F603F2ABA8B1"),
							DateOfBirth = DateTime.Parse("2002/03/10"),
							Gender = GenderOptions.Male.ToString(),
							ReceiveNewsLetters = true
						},
						new Person
						{
							Id = Guid.Parse("C58DC233-6A37-42B8-BDE1-A8B9A2420A09"),
							PersonName = "Naif Ahmed",
							Email = "n@n.com",
							Address = "Australia melbourne",
							CountryId = Guid.Parse("F225CCCA-10C7-44BD-886A-8D0EA28ED1C3"),
							DateOfBirth = DateTime.Parse("2000/08/10"),
							Gender = GenderOptions.Male.ToString(),
							ReceiveNewsLetters = true
						},
						new Person
						{
							Id = Guid.Parse("E4D8086F-838C-42E4-8F51-50C4C2975CB5"),
							PersonName = "Gidaa Zakaria",
							Email = "d@d.com",
							Address = "Australia melbourne",
							CountryId = Guid.Parse("F225CCCA-10C7-44BD-886A-8D0EA28ED1C3"),
							DateOfBirth = DateTime.Parse("1981/01/10"),
							Gender = GenderOptions.Female.ToString(),
							ReceiveNewsLetters = true
						},
						new Person
						{
							Id = Guid.Parse("60C318D6-94F0-4424-8D0D-C076E6AACD4F"),
							PersonName = "Anna Ahmed",
							Email = "c@c.com",
							Address = "Canada, Ontario",
							CountryId = Guid.Parse("B3E3C9A0-0925-4493-9E24-569C89A58EAD"),
							DateOfBirth = DateTime.Parse("1982/10/10"),
							Gender = GenderOptions.Female.ToString(),
							ReceiveNewsLetters = true
						},
						new Person
						{
							Id = Guid.Parse("64CE3461-96B5-4F6E-A091-7464D78F8142"),
							PersonName = "Aiham Khidr",
							Email = "k@k.com",
							Address = "South Korea, Seoul",
							CountryId = Guid.Parse("079C9AF0-BEA6-4407-B4CB-C960E8CEB4B6"),
							DateOfBirth = DateTime.Parse("1993/10/10"),
							Gender = GenderOptions.Male.ToString(),
							ReceiveNewsLetters = true
						}
				});
			}
		}

		public PersonResponse Add(PersonAddRequest person)
		{
			if (person == null) throw new ArgumentNullException(nameof(person));
			if (person.PersonName is null) throw new ArgumentException(nameof(person.PersonName));

			var personToAdd = _mapper.Map<Person>(person);
			personToAdd.Id = Guid.NewGuid();
			_persons.Add(personToAdd);

			var personResponse = _mapper.Map<PersonResponse>(personToAdd);

			return personResponse;
		}

		public bool Delete(Guid id)
		{
			if (id == Guid.Empty)
				return false;

			var person = _persons.Find(per => per.Id == id);
			if (person is null)
				return false;
			_persons.Remove(person);
			return true;
		}

		public List<PersonResponse> GetAll()
		{
			var allPersons = _mapper.Map<List<PersonResponse>>(_persons);

			PersonWithCountry(allPersons);
			return allPersons;
		}

		private void PersonWithCountry(List<PersonResponse> allPersons)
		{
			foreach (var item in allPersons)
			{
				item.Country = _countryService.GetById(item.CountryId)?.CountryName;
			}
		}

		public PersonResponse GetById(Guid id)
		{
			if(id == Guid.Empty)
				throw new ArgumentNullException("id");

			var person = _mapper.Map<PersonResponse>(_persons.Find(x => x.Id == id));
			person.Country = _countryService.GetById(person.CountryId)?.CountryName;
			return person;
		}

		public List<PersonResponse> GetFilteredPerson(string searchBy, string SearchString)
		{
			var allPersons = _mapper.Map<List<PersonResponse>>(_persons.ToList());
			PersonWithCountry(allPersons);
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
					 allPersons.Where(person => person.Gender.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
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
				allPerson.OrderBy(person => person.PersonName).ToList(),
				(nameof(PersonResponse.Email), SortOptions.DESCENDING) =>
				allPerson.OrderByDescending(person => person.PersonName).ToList(),

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

			};

			return sortedPersons;
		}

		public PersonResponse Update(PersonUpdateRequest request)
		{
			if (request is null)
				throw new ArgumentNullException();

			var person = _persons.Find(per => per.Id == request.Id);
			if (person == null)
				throw new ArgumentException("The given ID doesn't exist");

			ValidationHelper.Validate(request);

			person.PersonName = request.PersonName;
			person.Email = request.Email;
			person.ReceiveNewsLetters = request.ReceiveNewsLetters;

			return _mapper.Map<PersonResponse>(person);


		}
	}
}
