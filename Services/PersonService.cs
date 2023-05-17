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
        public PersonService(IMapper mapper)
        {
            _persons = new();
            _mapper = mapper;
            _countryService = new CountryService(mapper);
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
            if(id == Guid.Empty)
                return false;

            var person = _persons.Find(per => per.Id == id);
            if (person is null)
                return false;   
            _persons.Remove(person);
            return true;    
        }

        public List<PersonResponse> GetAll()
        {
            return _mapper.Map<List<PersonResponse>>(_persons);
        }

        public PersonResponse GetById(Guid id)
        {
            return _mapper.Map<PersonResponse>(_persons.Find(x => x.Id == id));
        }

        public List<PersonResponse> GetFilteredPerson(string searchBy, string SearchString)
        {
            var allPerson = _mapper.Map<List<PersonResponse>>(_persons.ToList());
            var matchingPersons = allPerson;

            if (string.IsNullOrWhiteSpace(searchBy))
                return matchingPersons;

            switch (searchBy)
            {
                case nameof(PersonResponse.PersonName):
                    matchingPersons = allPerson.Where(person => person.PersonName.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
                       .Select(p => _mapper.Map<PersonResponse>(p))
                       .ToList();
                    break;

                case nameof(PersonResponse.Email):
                    matchingPersons = allPerson.Where(person => person.Email.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
                       .Select(p => _mapper.Map<PersonResponse>(p))
                       .ToList();
                    break;
                case nameof(PersonResponse.Age):
                    //int age = 0;
                    matchingPersons = allPerson.Where(person => int.TryParse(SearchString, out int age) ? person.Age == age : person.Age == 0)
                       .Select(p => _mapper.Map<PersonResponse>(p))
                       .ToList();
                    break;

                case nameof(PersonResponse.DateOfBirth):
                    matchingPersons = allPerson.Where(person => (person.DateOfBirth != null) ?
                     person.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(SearchString) : true)
                       .Select(p => _mapper.Map<PersonResponse>(p))
                       .ToList();
                    break;

                case nameof(PersonResponse.Gender):
                    matchingPersons = allPerson.Where(person => person.Gender.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
                       .Select(p => _mapper.Map<PersonResponse>(p))
                       .ToList();
                    break;


                case nameof(PersonResponse.Address):
                    matchingPersons = allPerson.Where(person => person.Gender.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
                       .Select(p => _mapper.Map<PersonResponse>(p))
                       .ToList();
                    break;

                default:
                    matchingPersons = allPerson; break;
            }

            return matchingPersons;
        }

        public List<PersonResponse> GetSortedPersons(List<PersonResponse> persons, string sortBy, SortOptions sortDir)
        {
            var allPerson = _mapper.Map<List<PersonResponse>>(_persons.ToList());

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
            person.Email    = request.Email;    
            person.ReceiveNewsLetters = request.ReceiveNewsLetters;

            return _mapper.Map<PersonResponse>(person);


        }
    }
}
