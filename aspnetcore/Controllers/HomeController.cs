using Microsoft.AspNetCore.Mvc;

namespace aspnetcore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {

            return View();
        }
    }
}
