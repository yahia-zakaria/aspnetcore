using AutoMapper;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using System.Collections.Generic;

namespace CrudTests
{
    public class CountryServiceTest
    {
        private readonly ICountryService _countryService;
        private MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CountryAddRequest, Country>();
            cfg.CreateMap<Country, CountryResponse>();
            cfg.CreateMap<PersonAddRequest, Person>()
    .ForMember(dest => dest.Gender, memberOptions => memberOptions.MapFrom(src => src.Gender.ToString()));
            cfg.CreateMap<Person, PersonResponse>();
        });


        //constructor
        public CountryServiceTest()
        {
            _countryService = new CountryService(new Mapper(config));
        }

        #region AddCountry
        //When adding a null CountryAddRequest it must throw ArgumentNullException 
        [Fact]
        public void Add_NullCountryAddRequest()
        {
            //arrange
            CountryAddRequest? request = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                _countryService.Add(request);
            });

        }

        //When adding a CountryAddRequest object with null CountryName it must throw ArgumentException 

        [Fact]
        public void Add_CountryNameIsNull()
        {
            //arrange
            CountryAddRequest? request = new() { CountryName = null };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _countryService.Add(request);
            });

        }

        //When adding a duplicate CountryName it must throw ArgumentException 

        [Fact]
        public void Add_DuplicateCountryName()
        {
            //arrange
            CountryAddRequest? request1 = new() { CountryName = "USA" };
            CountryAddRequest? request2 = new() { CountryName = "USA" };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _countryService.Add(request1);
                _countryService.Add(request2);
            });

        }

        //When adding a proper CountryAddRequest it must be added to Countries list 

        [Fact]
        public void Add_ProperCountryAddRequest()
        {
            //arrange
            CountryAddRequest? request = new() { CountryName = "Japan" };

            //Act
            var countryAddResponse = _countryService.Add(request);

            //Assert
            Assert.True(countryAddResponse.Id != Guid.Empty);

        }

        [Fact]
        public void Add_AddingAfewCountries()
        {
            //arrange
            List<CountryAddRequest> countryAddRequests = new List<CountryAddRequest>()
            {
                new CountryAddRequest{CountryName = "USA"},
                new CountryAddRequest{CountryName = "Sudan"},
                new CountryAddRequest{CountryName = "Japan"},
            };
            List<CountryResponse> countryAddResponses = new List<CountryResponse>();

            //Act
            foreach (var item in countryAddRequests)
            {
                countryAddResponses.Add(_countryService.Add(item));
            }
            List<CountryResponse> countries = _countryService.GetAll().ToList();

            //Assert 
            foreach (var item in countryAddResponses)
            {
                Assert.Contains(item, countries);
            }

        }
        #endregion

        #region GetAll
        //when we call GetAll() we should receive a non-empty list of countries
        [Fact]
        public void GetAll_ReceiveNonEmptyList()
        {
            //Act
            List<CountryResponse> countries = _countryService.GetAll().ToList();

            //Arrange 
            Assert.NotEmpty(countries);
        }
        #endregion

        #region GetById
        [Fact]
        public void GetById_NullCountryId()
        {
            //arrange
            var id = Guid.Empty;

            //act
            var country = _countryService.GetById(id);

            //assert
            Assert.Null(country);
        }

        [Fact]
        public void GetById_ValidCountryId()
        {
            //arrange
            var request = new CountryAddRequest { CountryName = "Sudan" };

            //act
            var countryResponse = _countryService.Add(request);
            var country = _countryService.GetById(countryResponse.Id);
            var countryRequestAsCountryResponse = new CountryResponse { Id = country.Id, CountryName = request.CountryName };

            //assert
            Assert.Equal(country, countryRequestAsCountryResponse);
        }
        #endregion

    }
}