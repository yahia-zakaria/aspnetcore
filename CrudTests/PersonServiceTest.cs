using ServiceContracts;
using Services;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using AutoMapper;
using Entities;
using Services.Mapping;
using Microsoft.EntityFrameworkCore;
using EntityFrameworkCoreMock;
using AutoFixture;

namespace CrudTests
{
    public class PersonServiceTest
    {
        private readonly IPersonService _personService;
        private readonly Fixture _fixture;
        private MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        public PersonServiceTest()
        {
            var countries = new List<Country>();
            var persons = new List<Person>();

            var dbContextMock = new DbContextMock<ApplicationDbContext>(
                new DbContextOptionsBuilder<ApplicationDbContext>().Options);

            var dbContext = dbContextMock.Object;

            dbContextMock.CreateDbSetMock<Country>(entity => entity.Countries, countries);
            dbContextMock.CreateDbSetMock<Person>(entity => entity.Persons, persons);

            CountryService countryService = new CountryService(dbContext, mapperConfiguration.CreateMapper());
            _personService = new PersonService(dbContext, mapperConfiguration.CreateMapper(), countryService);

            _fixture = new Fixture();
        }

        #region Add
        //when supply null value as PersonAddRequest object it should throw ArgumentNullException 
        [Fact]
        public async Task Add_NullPersonAddRequest()
        {
            //arrange
            PersonAddRequest? person = null;

            //assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                //act
                var personResponse = await _personService.Add(person);
            });

        }

        //when supply a null PersonName object it should throw ArgumentException 
        [Fact]
        public async Task Add_NullPersonName()
        {
            //arrange
            PersonAddRequest? person = _fixture.Build<PersonAddRequest>().With(temp=>temp.PersonName, null as string).Create();

            //assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //act
                var personResponse = await _personService.Add(person);
            });

        }

        //when supply a proper PersonAddRequest object it add it to Persons list
        [Fact]
        public async Task Add_ProperPersonAddRequest()
        {
            //arrange
            PersonAddRequest? person = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.Email, "someone1@example.com")
                .With(temp => temp.TIN, "ABC12345")
                .Create();

            //act
            var personResponse = await _personService.Add(person);
            var persons = await _personService.GetAll();

            //assert
            Assert.True(personResponse.Id != Guid.Empty);
            Assert.Contains(personResponse, persons);

        }
        #endregion

        #region GetById
        [Fact]
        public async Task GetById_NullPersonId()
        {
            //arrange
            var person = new Person() { Id = Guid.Empty };

            //assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                //act
                var personResponse = await _personService.GetById(person.Id);
            });

        }

        [Fact]
        public async Task GetById_ProperPersonId()
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
                ReceiveNewsLetters = true,
                TIN = "ABC12345"
            };

            //act
            var PersonResponseFromAdd = await _personService.Add(personToSearch);
            var personsResponse = await _personService.GetById(PersonResponseFromAdd.Id);

            //assert 
            Assert.Equal(PersonResponseFromAdd, personsResponse);

        }
        #endregion

        #region GetAll()
        [Fact]
        public async Task GetAll_EmptyList()
        {
            //act 
            var persons = await _personService.GetAll();


            //assert
            Assert.Empty(persons);
        }

        [Fact]
        public async Task GetAll_AddFewPersons()
        {
            //arrange
            PersonAddRequest? person1 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.Email, "someone1@example.com")
                .With(temp => temp.TIN, "ABC12345")
                .Create();

            PersonAddRequest? person2 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.Email, "someone2@example.com")
                .With(temp => temp.TIN, "ABC12345")
                .Create();


            //act
            var person1ResponseFromAdd = await _personService.Add(person1);
            var person2ResponseFromAdd = await _personService.Add(person2);
            var allPersons = await _personService.GetAll();

            //assert 
            Assert.Contains(person1ResponseFromAdd, allPersons);
            Assert.Contains(person2ResponseFromAdd, allPersons);
        }
        #endregion

        #region GetFilteredPersons
        [Fact]
        public async Task GetFilteredPersons_EmptySearchBy()
        {
            //arrange
            string searchBy = string.Empty;
            string searchString = string.Empty;

            //act 
            var filteredPersons = await _personService.GetFilteredPerson(searchBy, searchString);

            //assert
            Assert.Empty(filteredPersons);

        }

        [Fact]
        public async Task GetFilteredPersons_WithSearchByAndSearchString()
        {
            //arrange
            PersonAddRequest? person1 = _fixture.Build<PersonAddRequest>()
               .With(temp => temp.Email, "someone1@example.com")
               .With(temp => temp.TIN, "ABC12345")
               .Create();

            PersonAddRequest? person2 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.Email, "someone2@example.com")
                .With(temp => temp.TIN, "ABC12345")
                .Create();

            //act
            var person1Response = await _personService.Add(person1);
            var person2Response = await _personService.Add(person2);
            List<PersonResponse> personResponsesFromAdd = new List<PersonResponse>();
            personResponsesFromAdd.Add(person1Response);
            personResponsesFromAdd.Add(person2Response);
            var filteredPersons = await _personService.GetFilteredPerson(nameof(Person.PersonName), "ya");
            var allPersons = await _personService.GetAll();

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
        public async Task GetSortedPerson_SortByPersonNameDescending()
        {
            PersonAddRequest? person1 = _fixture.Build<PersonAddRequest>()
          .With(temp => temp.Email, "someone1@example.com")
          .With(temp => temp.TIN, "ABC12345")
          .Create();

            PersonAddRequest? person2 = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.Email, "someone2@example.com")
                .With(temp => temp.TIN, "ABC12345")
                .Create();

            //act
            var person1Response = await _personService.Add(person1);
            var person2Response = await _personService.Add(person2);
            List<PersonResponse> personResponsesFromAdd = new List<PersonResponse>();
            personResponsesFromAdd.Add(person1Response);
            personResponsesFromAdd.Add(person2Response);

            var sortedPersonsFromAdd = _personService.GetSortedPersons(personResponsesFromAdd, nameof(PersonResponse.PersonName), SortOptions.DESCENDING);
            var sortedPersonsFromGetAll = _personService.GetSortedPersons(
                await _personService.GetAll(), nameof(PersonResponse.PersonName), SortOptions.DESCENDING);

            for (int i = 0; i < sortedPersonsFromGetAll.Count; i++)
            {
                //assert
                Assert.True(sortedPersonsFromAdd[i].Id == sortedPersonsFromGetAll[i].Id);
            }
        }
        #endregion

        #region Update
        [Fact]
        public async Task Update_NullPersonUpdateRequest()
        {
            PersonUpdateRequest personUpdateRequest = null;

            //assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                //act
                await _personService.Update(personUpdateRequest);
            });
        }

        [Fact]
        public async Task Update_PersonUpdateRequestWithFullDetails()
        {
            //arrange
            PersonAddRequest? personToAdd = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.Email, "someone1@example.com")
                .With(temp => temp.TIN, "ABC12345")
                .Create();

            PersonUpdateRequest? personToUpdate = _fixture.Build<PersonUpdateRequest>()
                .With(temp => temp.Email, "someone2@example.com")
                .With(temp => temp.TIN, "ABC12345")
                .Create();

            //act
            var person_from_add = await _personService.Add(personToAdd);
            personToUpdate.Id = person_from_add.Id;
            var person_after_update = await _personService.Update(personToUpdate);
            var person_from_getById = await _personService.GetById(person_after_update.Id);

            //assert
            Assert.Equal(person_from_getById, person_after_update);
        }
        #endregion

        #region Delete
        [Fact]
        public async Task Delete_InvalidPersonId()
        {
            var personId = Guid.Empty;

            //act   
            var isDeleted = await _personService.Delete(personId);

            //assert
            Assert.False(isDeleted);
        }

        [Fact]
        public async Task Delete_ValidPersonId()
        {
            PersonAddRequest? person = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.Email, "someone1@example.com")
                .With(temp => temp.TIN, "ABC12345")
                .Create();

            //act   
            var personResponse = await _personService.Add(person);
            var isDeleted = await _personService.Delete(personResponse.Id);

            //assert
            Assert.True(isDeleted);
            Assert.DoesNotContain(personResponse, await _personService.GetAll());
        }
        #endregion
    }
}
