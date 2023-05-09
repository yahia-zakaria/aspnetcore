using AutoMapper;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
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

        public List<PersonResponse> GetAll()
        {
            return _mapper.Map<List<PersonResponse>>(_persons);
        }
    }
}
