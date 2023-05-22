using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;

namespace aspnetcore.Controllers
{
	public class PersonsController : Controller
	{
		private readonly IPersonService _personservice;
		private readonly ICountryService _countryService;

		public PersonsController(IPersonService personservice, ICountryService countryService)
		{
			_personservice = personservice;
			_countryService = countryService;
		}

		public IActionResult Index(string searchBy, string searchString,
			string sortBy = nameof(PersonResponse.PersonName), SortOptions sortDir = SortOptions.ASCENDING)
		{
			ViewBag.searchFields = new Dictionary<string, string>()
			{
				{nameof(PersonResponse.PersonName), "Person Name" },
				{nameof(PersonResponse.Email), "Email" },
				{nameof(PersonResponse.Age), "Age" },
				{nameof(PersonResponse.DateOfBirth), "DateOfBirth" },
				{nameof(PersonResponse.Gender), "Gender" },
				{nameof(PersonResponse.Country), "Country" },
				{nameof(PersonResponse.Address), "Address" }
			};
			var persons = _personservice.GetFilteredPerson(searchBy, searchString);

			var sortedPersons = _personservice.GetSortedPersons(persons, sortBy, sortDir);

			ViewBag.currentSearchBy = searchBy;
			ViewBag.currentSearchString = searchString;
			ViewBag.currentSortBy = sortBy;
			ViewBag.currentSortDir = sortDir;

			return View(sortedPersons);
		}

		[HttpGet]
		public IActionResult Create()
		{
			ViewBag.countries = new SelectList(_countryService.GetAll(), "Id", "CountryName");
			return View();
		}

		[HttpPost]
		public IActionResult Create(PersonAddRequest personAddRequest)
		{
			if(!ModelState.IsValid)
			{
				ViewBag.countries = new SelectList(_countryService.GetAll(), "Id", "CountryName");
				return View();
			}
			var response = _personservice.Add(personAddRequest);
			return RedirectToAction("Index", "Persons");
		}
	}
}
