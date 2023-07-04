using aspnetcore.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts;

namespace aspnetcore.Filters.ActionFilters
{
    public class PersonCreateEditPostActionFilter : IAsyncActionFilter
    {
        private readonly ICountryService _countryService;

        public PersonCreateEditPostActionFilter(ICountryService countryService)
        {
            _countryService = countryService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if(context.Controller is PersonsController personsController)
            {
                if (!personsController.ModelState.IsValid)
                {
                    personsController.ViewBag.countries = new SelectList(await _countryService.GetAll(), "Id", "CountryName");
                    var model = context.ActionArguments["model"];
                    context.Result = personsController.View(model);
                }
                else
                {
                    await next();
                }
            }
            else
            {
                await next();
            }
        }
    }
}
