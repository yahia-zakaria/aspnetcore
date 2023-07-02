using aspnetcore.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace aspnetcore.Filters.ActionFilters
{
    public class PersonsListActionFilter : IActionFilter
    {
        private readonly ILogger<PersonsListActionFilter> _logger;

        public PersonsListActionFilter(ILogger<PersonsListActionFilter> logger)
        {
            _logger = logger;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("PersonsListActionFilter - {0} OnActionExecuted", context.ActionDescriptor.DisplayName);
            PersonsController personsController = (PersonsController)context.Controller;
            if (context.HttpContext.Items.ContainsKey("arguments"))
            {
                Dictionary<string, object> args = (Dictionary<string, object>)context.HttpContext.Items["arguments"];
                if (args != null)
                {
                    if (args.ContainsKey("searchBy"))
                    {
                        personsController.ViewBag.currentSearchBy = Convert.ToString(args["searchBy"]);
                    }

                    if (args.ContainsKey("searchString"))
                    {
                        personsController.ViewBag.currentSearchString = Convert.ToString(args["searchString"]);
                    }

                    if (args.ContainsKey("sortBy"))
                    {
                        personsController.ViewBag.currentsortBy = Convert.ToString(args["sortBy"]);
                    }

                    if (args.ContainsKey("sortDir"))
                    {
                        personsController.ViewBag.currentsortDir = (SortOptions)args["sortDir"];
                    }
                }
            }

            personsController.ViewBag.searchFields = new Dictionary<string, string>()
            {
                {nameof(PersonResponse.PersonName), "Person Name" },
                {nameof(PersonResponse.Email), "Email" },
                {nameof(PersonResponse.Age), "Age" },
                {nameof(PersonResponse.DateOfBirth), "DateOfBirth" },
                {nameof(PersonResponse.Gender), "Gender" },
                {nameof(PersonResponse.Country), "Country" },
                {nameof(PersonResponse.Address), "Address" }
            };
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Items["arguments"] = context.ActionArguments;
            object searchByString = "";
            _logger.LogInformation("PersonsListActionFilter-{0} OnActionExecuting", context.ActionDescriptor.DisplayName);
            if (context.ActionArguments.ContainsKey("searchBy")
                && context.ActionArguments.TryGetValue("searchBy", out searchByString))
            {
                var searchBy = (string)searchByString;
                var searchByOptions = new List<string>();
                foreach (var property in typeof(PersonResponse).GetProperties())
                {
                    searchByOptions.Add(property.Name);
                }

                if (!searchByOptions.Any(prop => prop.Equals(searchBy)))
                {
                    _logger.LogInformation("searchBy actual value {0}", searchBy);
                    context.ActionArguments["searchBy"] = nameof(PersonResponse.PersonName);
                    _logger.LogInformation("searchBy updated value {0}", context.ActionArguments["searchBy"]);

                }
            }
        }
    }
}
