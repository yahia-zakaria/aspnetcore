using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace aspnetcore.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Error()
		{
			IExceptionHandlerPathFeature exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
			ViewBag.ErrorMessage = exceptionHandlerPathFeature is not null && exceptionHandlerPathFeature.Error is not null ?
				exceptionHandlerPathFeature.Error.Message : null;
			return View();
		}
	}
}
