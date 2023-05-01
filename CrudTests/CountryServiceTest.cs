using ServiceContracts;
using ServiceContracts.DTO;
using Services;

namespace CrudTests
{
    public class CountryServiceTest
    {
        private readonly ICountryService _countryService;

        public CountryServiceTest()
        {
            _countryService = new CountryService();
        }

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
            CountryAddRequest? request = new() { CountryName = null};

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
            Assert.True(countryAddResponse.CountryId != Guid.Empty);

        }
    }
}