﻿using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Filters;

namespace aspnetcore.Filters.ActionFilters
{
    public class ReponseHeaderActionFilter : ActionFilterAttribute
    {
        private readonly string _key;
        private readonly string _value;

        public ReponseHeaderActionFilter(string key, string value, int order)
        {
            _key = key;
            _value = value;
            Order = order;

        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            await next();
            context.HttpContext.Response.Headers[_key] = _value;
        }
    }
}
