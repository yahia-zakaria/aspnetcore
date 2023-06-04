﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
using Rotativa.AspNetCore.Options;

namespace aspnetcore.Controllers
{
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

		public async Task<IActionResult> Index(string searchBy, string searchString,
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
			var persons = await _personservice.GetFilteredPerson(searchBy, searchString);

			var sortedPersons = _personservice.GetSortedPersons(persons, sortBy, sortDir);

			ViewBag.currentSearchBy = searchBy;
			ViewBag.currentSearchString = searchString;
			ViewBag.currentSortBy = sortBy;
			ViewBag.currentSortDir = sortDir;

			return View(sortedPersons);
		}

		[HttpGet]
		public async Task<IActionResult> Create()
		{
			ViewBag.countries = new SelectList(await _countryService.GetAll(), "Id", "CountryName");
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(PersonAddRequest personAddRequest)
		{
			if (!ModelState.IsValid)
			{
				ViewBag.countries = new SelectList(await _countryService.GetAll(), "Id", "CountryName");
				return View("Create", personAddRequest);
			}
			var response = await _personservice.Add(personAddRequest);
			return RedirectToAction("Index", "Persons");
		}

		[HttpGet]
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
		public async Task<IActionResult> Edit(PersonUpdateRequest personUpdateRequest)
		{
			if (!ModelState.IsValid)
			{
				ViewBag.countries = new SelectList(await _countryService.GetAll(), "Id", "CountryName");
				return View(personUpdateRequest);
			}
			await _personservice.Update(personUpdateRequest);
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
