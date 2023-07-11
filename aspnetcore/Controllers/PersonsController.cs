using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
using Rotativa.AspNetCore.Options;
using SerilogTimings;
using aspnetcore.Filters.ActionFilters;
using aspnetcore.Filters.ResultFilters;
using aspnetcore.Filters.ResourceFilters;
using aspnetcore.Filters.AuthorizationFilters;
using aspnetcore.Filters.ExceptionFilters;
using aspnetcore.Filters.SkipFilters;



namespace aspnetcore.Controllers
{
    [ReponseHeaderActionFilter("X-Custom-Key-FromController", "Custom-Value-FromController", 2)]
    //[TypeFilter(typeof(HandleExceptionFilter))]
    public class PersonsController : Controller
    {
        private readonly IPersonService _personservice;
        private readonly ICountryService _countryService;
        private readonly IMapper _mapper;

        public PersonsController(IPersonService personservice, ICountryService countryService, IMapper mapper)
        {
            _personservice = personservice;
            _countryService = countryService;
            _mapper = mapper;
        }

        [TypeFilter(typeof(PersonsListActionFilter))]
        [ReponseHeaderActionFilter("X-Custom-Key-FromAction", "Custom-Value-FromAction", 1)]
        [TypeFilter(typeof(PersonsListResultFilter))]
        [TypeFilter(typeof(TokenResultFilter))]
        public async Task<IActionResult> Index(string searchBy, string searchString,
            string sortBy = nameof(PersonResponse.PersonName), SortOptions sortDir = SortOptions.ASCENDING)
        {
            List<PersonResponse> persons = new List<PersonResponse>();
            List<PersonResponse> sortedPersons = new List<PersonResponse>();

            using (Operation.Time("Time taken for filtering and sorting persons"))
            {
                persons = await _personservice.GetFilteredPerson(searchBy, searchString);

                sortedPersons = _personservice.GetSortedPersons(persons, sortBy, sortDir);
            }


            return View(sortedPersons);
        }

        [HttpGet]
        [TypeFilter(typeof(DisableFeatureResourceFilter), Arguments = new object[] { false })]
        public async Task<IActionResult> Create()
        {
            ViewBag.countries = new SelectList(await _countryService.GetAll(), "Id", "CountryName");
            return View();
        }

        [HttpPost]
        [TypeFilter(typeof(PersonCreateEditPostActionFilter))]
        public async Task<IActionResult> Create(PersonAddRequest model)
        {
            var response = await _personservice.Add(model);
            return RedirectToAction("Index", "Persons");
        }

        [HttpGet]
        [TypeFilter(typeof(PersonAlwaysRunResultFilter))]
        [SkipAlwaysRunResultFilter]
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            var person = await _personservice.GetById(id);
            if (person == null)
            {
                return NotFound();
            }

            ViewBag.countries = new SelectList(await _countryService.GetAll(), "Id", "CountryName");
            return View(_mapper.Map<PersonUpdateRequest>(person));
        }

        [HttpPost]
        [TypeFilter(typeof(PersonCreateEditPostActionFilter))]
        //[TypeFilter(typeof(TokenAuthorizationFilter))]
        public async Task<IActionResult> Edit(PersonUpdateRequest model)
        {
            model.Id = Guid.NewGuid();  
            await _personservice.Update(model);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            var person = await _personservice.GetById(id);
            if (person == null)
            {
                return NotFound();
            }

            ViewBag.countries = new SelectList(await _countryService.GetAll(), "Id", "CountryName");
            return View(person);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }
            await _personservice.Delete(id);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> PersonsPDF()
        {
            //Get list of persons
            List<PersonResponse> persons = await _personservice.GetAll();

            //Return view as pdf
            return new ViewAsPdf("PersonsPDF", persons, ViewData)
            {
                PageMargins = new Margins() { Top = 20, Right = 20, Bottom = 20, Left = 20 },
                PageOrientation = Orientation.Landscape
            };
        }
        public async Task<IActionResult> PersonsCSV()
        {
            MemoryStream memoryStream = await _personservice.GetPersonsCSV();
            return File(memoryStream, "application/octet-stream", "persons.csv");
        }
        public async Task<IActionResult> PersonsExcel()
        {
            MemoryStream memoryStream = await _personservice.GetPersonsExcel();
            return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "persons.xlsx");
        }
	}
}
