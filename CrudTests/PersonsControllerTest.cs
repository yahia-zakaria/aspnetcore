using aspnetcore.Controllers;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudTests
{
    public class PersonsControllerTest
    {
        private MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });
        private readonly IPersonService _personsService;
        private readonly ICountryService _countriesService;

        private readonly Mock<ICountryService> _countriesServiceMock;
        private readonly Mock<IPersonService> _personsServiceMock;

        private readonly Fixture _fixture;
        public PersonsControllerTest()
        {
            _countriesServiceMock = new Mock<ICountryService>();
            _personsServiceMock = new Mock<IPersonService>();

            _countriesService = _countriesServiceMock.Object;
            _personsService = _personsServiceMock.Object;
            _fixture = new Fixture();
        }

        #region Index_Action

        [Fact]
        public async Task Index_ShouldReturnIndexViewWithPersonsList()
        {
            //arrange
            var personsList = _fixture.Create<List<PersonResponse>>();
            PersonsController personsController = new(_personsService, _countriesService, mapperConfiguration.CreateMapper());

            #region mocking_servicers_methods
            //mocking GetFilteredPerson
            _personsServiceMock.Setup(temp=>temp.GetFilteredPerson(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(personsList);

            //mocking GetSortedPersons
            _personsServiceMock.Setup(temp=>temp.GetSortedPersons(It.IsAny<List<PersonResponse>>(), It.IsAny<string>(), It.IsAny<SortOptions>()))
                .Returns(personsList);

            #endregion

            //Act
            IActionResult result = await personsController.Index(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<SortOptions>());

            //assert 
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            viewResult.ViewData.Model.Should().BeAssignableTo<IEnumerable<PersonResponse>>();
            viewResult.Model.Should().BeEquivalentTo(personsList);
        }

        #endregion

        #region Create_Action
        //[Fact]
        //public async Task Create_IfModelErrors_ToReturnCreateView()
        //{
        //    //arrange
        //    var personAddRequest = _fixture.Create<PersonAddRequest>();
        //    PersonsController personsController = new(_personsService, _countriesService, mapperConfiguration.CreateMapper());
        //    PersonResponse personResponse = _fixture.Create<PersonResponse>();
        //    List<CountryResponse> countyries = _fixture.Create<List<CountryResponse>>();

        //    _personsServiceMock.Setup(temp => temp.Add(It.IsAny<PersonAddRequest>()))
        //        .ReturnsAsync(personResponse);

        //    _countriesServiceMock.Setup(temp=>temp.GetAll())
        //        .ReturnsAsync(countyries);

        //    personsController.ModelState.AddModelError("PersonName", "error");

        //    //act 
        //    IActionResult result = await  personsController.Create(personAddRequest);

        //    ViewResult viewResult = Assert.IsType<ViewResult>(result);
        //    viewResult.ViewData.Model.Should().BeAssignableTo<PersonAddRequest>();
        //    viewResult.ViewName.Should().Be("Create");

        //}

        [Fact]
        public async Task Create_IfNoModelErrors_ToReturnCreateView()
        {
            //arrange
            var personAddRequest = _fixture.Create<PersonAddRequest>();
            PersonsController personsController = new(_personsService, _countriesService, mapperConfiguration.CreateMapper());
            PersonResponse personResponse = _fixture.Create<PersonResponse>();
            List<CountryResponse> personResponseList = _fixture.Create<List<CountryResponse>>();

            _personsServiceMock.Setup(temp => temp.Add(It.IsAny<PersonAddRequest>()))
                .ReturnsAsync(personResponse);

            _countriesServiceMock.Setup(temp => temp.GetAll())
                .ReturnsAsync(personResponseList);

            //act 
            IActionResult result = await personsController.Create(personAddRequest);

            RedirectToActionResult redirectResult = Assert.IsType<RedirectToActionResult>(result);
            redirectResult.ActionName.Should().Be("Index");

        }
        #endregion
    }
}
