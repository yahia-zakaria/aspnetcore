using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

namespace aspnetcore.Controllers
{
    public class PersonsController : Controller
    {
        private readonly IPersonService _personservice;

		public PersonsController(IPersonService personservice)
		{
			_personservice = personservice;
		}

		public IActionResult Index()
        {
            var persons = _personservice.GetAll();  
            return View(persons);
        }
    }
}
