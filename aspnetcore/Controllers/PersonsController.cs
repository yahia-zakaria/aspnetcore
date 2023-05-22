using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace aspnetcore.Controllers
{
    public class PersonsController : Controller
    {
        private readonly IPersonService _personservice;

		public PersonsController(IPersonService personservice)
		{
			_personservice = personservice;
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
    }
}
