using AutoMapper;
using Entities;
using EntityFrameworkCoreMock;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using Services.Mapping;

namespace CrudTests
{
    public class CountryServiceTest
    {
        private readonly ICountryService _countryService;
        private MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg =>
        {
			cfg.AddProfile(new MappingProfile());
		});


        //constructor
        public CountryServiceTest()
        {
            var countries = new List<Country>();
            var dbContextMock = new DbContextMock<ApplicationDbContext>(
                new DbContextOptionsBuilder<ApplicationDbContext>().Options);

            var dbContext = dbContextMock.Object;

            dbContextMock.CreateDbSetMock<Country>(entity => entity.Countries, countries);

            _countryService = new CountryService(dbContext, mapperConfiguration.CreateMapper());
        }

        #region AddCountry
        //When adding a null CountryAddRequest it must throw ArgumentNullException 
        [Fact]
        public async Task Add_NullCountryAddRequest()
        {
            //arrange
            CountryAddRequest? request = null;

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                //Act
                await _countryService.Add(request);
            });

        }

        //When adding a CountryAddRequest object with null CountryName it must throw ArgumentException 

        [Fact]
        public async Task Add_CountryNameIsNull()
        {
            //arrange
            CountryAddRequest? request = new() { CountryName = null };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _countryService.Add(request);
            });

        }

        //When adding a duplicate CountryName it must throw ArgumentException 

        [Fact]
        public async Task Add_DuplicateCountryName()
        {
            //arrange
            CountryAddRequest? request1 = new() { CountryName = "USA" };
            CountryAddRequest? request2 = new() { CountryName = "USA" };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _countryService.Add(request1);
                await _countryService.Add(request2);
            });

        }

        //When adding a proper CountryAddRequest it must be added to Countries list 

        [Fact]
        public async Task Add_ProperCountryAddRequest()
        {
            //arrange
            CountryAddRequest? request = new() { CountryName = "Japan" };

            //Act
            var countryAddResponse = await _countryService.Add(request);

            //Assert
            Assert.True(countryAddResponse.Id != Guid.Empty);

        }

        [Fact]
        public async Task Add_AddingAfewCountries()
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
                countryAddResponses.Add(await _countryService.Add(item));
            }
            var result = await _countryService.GetAll();
			List<CountryResponse> countries = result.ToList();
			//Assert 
			foreach (var item in countryAddResponses)
            {
                Assert.Contains(item, countries);
            }

        }
        #endregion

        #region GetAll
        //when we call GetAll() we should receive empty list of countries
        [Fact]
        public async Task GetAll_ReceiveEmptyList()
        {
			//Act
			var result = await _countryService.GetAll();
			List<CountryResponse> countries = result.ToList();
			//Arrange 
			Assert.Empty(countries);
        }
        #endregion

        #region GetById
        [Fact]
        public async Task GetById_NullCountryId()
        {
            //arrange
            var id = Guid.Empty;

            //act
            var country = await _countryService.GetById(id);

            //assert
            Assert.Null(country);
        }

        [Fact]
        public async Task GetById_ValidCountryId()
        {
            //arrange
            var request = new CountryAddRequest { CountryName = "Sudan" };

            //act
            var countryResponse =  await _countryService.Add(request);
            var country = await _countryService.GetById(countryResponse.Id);
            var countryRequestAsCountryResponse = new CountryResponse { Id = country.Id, CountryName = request.CountryName };

            //assert
            Assert.Equal(country, countryRequestAsCountryResponse);
        }
        #endregion

    }
}