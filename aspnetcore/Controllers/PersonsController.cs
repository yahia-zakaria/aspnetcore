using Microsoft.AspNetCore.Mvc;

namespace aspnetcore.Controllers
{
    public class PersonsController : Controller
    {
        public IActionResult Index()
        {

            return View();
        }
    }
}
