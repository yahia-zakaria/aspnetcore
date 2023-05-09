using ServiceContracts;
using Xunit;
using Services;
using System.Diagnostics.CodeAnalysis;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using AutoMapper;
using Entities;

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
    }
}
