using ServiceContracts;
using Xunit;
using Services;
using System.Diagnostics.CodeAnalysis;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using AutoMapper;
using Entities;
using Xunit.Sdk;

namespace CrudTests
{
    public class PersonServiceTest
    {
        private readonly IPersonService _personService;
        private MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CountryAddRequest, Country>();
            cfg.CreateMap<Country, CountryResponse>();
            cfg.CreateMap<PersonAddRequest, Person>()
            .ForMember(dest => dest.Gender, memberOptions => memberOptions.MapFrom(src => src.Gender.ToString()));
            cfg.CreateMap<Person, PersonResponse>();
        });

        public PersonServiceTest()
        {
            _personService = new PersonService(new Mapper(config));
        }

        #region Add
        //when supply null value as PersonAddRequest object it should throw ArgumentNullException 
        [Fact]
        public void Add_NullPersonAddRequest()
        {
            //arrange
            PersonAddRequest? person = null;

            //assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //act
                var personResponse = _personService.Add(person);
            });

        }

        //when supply a null PersonName object it should throw ArgumentException 
        [Fact]
        public void Add_NullPersonName()
        {
            //arrange
            PersonAddRequest? person = new PersonAddRequest { PersonName = null };

            //assert
            Assert.Throws<ArgumentException>(() =>
            {
                //act
                var personResponse = _personService.Add(person);
            });

        }

        //when supply a proper PersonAddRequest object it add it to Persons list
        [Fact]
        public void Add_ProperPersonAddRequest()
        {
            //arrange
            PersonAddRequest? person = new PersonAddRequest
            {
                PersonName = "Yahia Zakaria",
                Email = "a@a.com",
                Address = "Australia melbourne",
                CountryId = Guid.NewGuid()
            ,
                DateOfBirth = DateTime.Parse("1991/01/10"),
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true
            };

            //act
            var personResponse = _personService.Add(person);
            var persons = _personService.GetAll();

            //assert
            Assert.True(personResponse.Id != Guid.Empty);
            Assert.Contains(personResponse, persons);

        }
        #endregion

        #region GetById
        [Fact]
        public void GetById_NullPersonId()
        {
            //arrange
            var person = new Person() { Id = Guid.Empty };

            //act
            var personResponse = _personService.GetById(person.Id);

            //assert
            Assert.Null(personResponse);

        }

        [Fact]
        public void GetById_ProperPersonId()
        {
            //arrange
            PersonAddRequest? personToSearch = new PersonAddRequest
            {
                PersonName = "Yahia Zakaria",
                Email = "a@a.com",
                Address = "Australia melbourne",
                CountryId = Guid.NewGuid()
      ,
                DateOfBirth = DateTime.Parse("1991/01/10"),
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true
            };

            //act
            var PersonResponseFromAdd = _personService.Add(personToSearch);
            var personsResponse = _personService.GetById(PersonResponseFromAdd.Id);

            //assert 
            Assert.Equal(PersonResponseFromAdd, personsResponse);

        }
        #endregion

        #region GetAll()
        [Fact]
        public void GetAll_EmptyList()
        {
            //act 
            var persons = _personService.GetAll();


            //assert
            Assert.Empty(persons);
        }

        [Fact]
        public void GetAll_AddFewPersons()
        {
            //arrange
            PersonAddRequest? person1 = new PersonAddRequest
            {
                PersonName = "Yahia Zakaria",
                Email = "a@a.com",
                Address = "Australia melbourne",
                CountryId = Guid.NewGuid()
  ,
                DateOfBirth = DateTime.Parse("1991/01/10"),
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true
            };

            PersonAddRequest? person2 = new PersonAddRequest
            {
                PersonName = "Ahmed Ali",
                Email = "a@a.com",
                Address = "Australia melbourne",
                CountryId = Guid.NewGuid()
,
                DateOfBirth = DateTime.Parse("1993/01/10"),
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true
            };


            //act
            var person1ResponseFromAdd = _personService.Add(person1);
            var person2ResponseFromAdd = _personService.Add(person2);
            var allPersons = _personService.GetAll();

            //assert 
            Assert.Contains(person1ResponseFromAdd, allPersons);
            Assert.Contains(person2ResponseFromAdd, allPersons);
        }
        #endregion

        #region GetFilteredPersons
        [Fact]
        public void GetFilteredPersons_EmptySearchBy()
        {
            //arrange
            string searchBy = string.Empty;
            string searchString = string.Empty;

            //act 
            var filteredPersons = _personService.GetFilteredPerson(searchBy, searchString);

            //assert
            Assert.Empty(filteredPersons);

        }

        [Fact]
        public void GetFilteredPersons_WithSearchByAndSearchString()
        {
            //arrange
            PersonAddRequest? person1 = new PersonAddRequest
            {
                PersonName = "Yahia Zakaria",
                Email = "a@a.com",
                Address = "Australia melbourne",
                CountryId = Guid.NewGuid()
,
                DateOfBirth = DateTime.Parse("1991/01/10"),
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true
            };

            PersonAddRequest? person2 = new PersonAddRequest
            {
                PersonName = "Ahmed Ali",
                Email = "a@a.com",
                Address = "Australia melbourne",
                CountryId = Guid.NewGuid()
,
                DateOfBirth = DateTime.Parse("1993/01/10"),
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true
            };

            //act
            var person1Response = _personService.Add(person1);
            var person2Response = _personService.Add(person2);
            List<PersonResponse> personResponsesFromAdd = new List<PersonResponse>();
            personResponsesFromAdd.Add(person1Response);
            personResponsesFromAdd.Add(person2Response);
            var filteredPersons = _personService.GetFilteredPerson(nameof(Person.PersonName), "ya");
            var allPersons = _personService.GetAll();

            foreach (var item in allPersons)
            {
                var person = item.PersonName.Contains("ya", StringComparison.OrdinalIgnoreCase);
                //assert 
                Assert.Contains(item, allPersons);
            }
        }

        #endregion

        #region GetSortedPerson
        [Fact]
        public void GetSortedPerson_SortByPersonNameDescending()
        {
            PersonAddRequest? person1 = new PersonAddRequest
            {
                PersonName = "Yahia Zakaria",
                Email = "a@a.com",
                Address = "Australia melbourne",
                CountryId = Guid.NewGuid()
,
                DateOfBirth = DateTime.Parse("1991/01/10"),
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true
            };

            PersonAddRequest? person2 = new PersonAddRequest
            {
                PersonName = "Ahmed Ali",
                Email = "a@a.com",
                Address = "Australia melbourne",
                CountryId = Guid.NewGuid()
,
                DateOfBirth = DateTime.Parse("1993/01/10"),
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true
            };

            //act
            var person1Response = _personService.Add(person1);
            var person2Response = _personService.Add(person2);
            List<PersonResponse> personResponsesFromAdd = new List<PersonResponse>();
            personResponsesFromAdd.Add(person1Response);
            personResponsesFromAdd.Add(person2Response);

            var sortedPersonsFromAdd = _personService.GetSortedPersons(personResponsesFromAdd, nameof(PersonResponse.PersonName), SortOptions.DESCENDING);
            var sortedPersonsFromGetAll = _personService.GetSortedPersons(
                _personService.GetAll(), nameof(PersonResponse.PersonName), SortOptions.DESCENDING);

            for (int i = 0; i < sortedPersonsFromGetAll.Count; i++)
            {
                //assert
                Assert.True(sortedPersonsFromAdd[i].Id == sortedPersonsFromGetAll[i].Id);
            }
        }
        #endregion

        #region Update
        [Fact]
        public void Update_NullPersonUpdateRequest()
        {
            PersonUpdateRequest personUpdateRequest = null;

            //assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //act
                _personService.Update(personUpdateRequest);
            });
        }

        [Fact]
        public void Update_PersonUpdateRequestWithFullDetails()
        {
            //arrange
            PersonAddRequest? personToAdd = new PersonAddRequest
            {
                PersonName = "Yahia Zakaria",
                Email = "a@a.com",
                Address = "Australia melbourne",
                CountryId = Guid.NewGuid()
,
                DateOfBirth = DateTime.Parse("1991/01/10"),
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true
            };

            PersonUpdateRequest? personToUpdate = new PersonUpdateRequest
            {
                PersonName = "Osman Zakaria",
                Email = "b@a.com",
                ReceiveNewsLetters = true
            };

            //act
            var person_from_add = _personService.Add(personToAdd);
            personToUpdate.Id = person_from_add.Id;
            var person_after_update = _personService.Update(personToUpdate);
            var person_from_getById = _personService.GetById(person_after_update.Id);

            //assert
            Assert.Equal(person_from_getById, person_after_update);
        }
        #endregion

        #region Delete
        [Fact]
        public void Delete_InvalidPersonId()
        {
            var personId = Guid.Empty;

            //act   
            var isDeleted = _personService.Delete(personId);
            
            //assert
            Assert.False(isDeleted);
        }

        [Fact]
        public void Delete_ValidPersonId()
        {
            PersonAddRequest? person = new PersonAddRequest
            {
                PersonName = "Yahia Zakaria",
                Email = "a@a.com",
                Address = "Australia melbourne",
                CountryId = Guid.NewGuid()
,
                DateOfBirth = DateTime.Parse("1991/01/10"),
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true
            };

            //act   
            var personResponse = _personService.Add(person);
            var isDeleted = _personService.Delete(personResponse.Id);

            //assert
            Assert.True(isDeleted);
            Assert.DoesNotContain(personResponse, _personService.GetAll());
        }
        #endregion
    }
}
