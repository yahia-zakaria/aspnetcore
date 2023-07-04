using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Filters;

namespace aspnetcore.Filters.ActionFilters
{
    public class ReponseHeaderActionFilter : IActionFilter, IOrderedFilter
    {
        private readonly ILogger<ReponseHeaderActionFilter> _logger;
        private readonly string _key;
        private readonly string _value;

        public int Order { get; set; }

        public ReponseHeaderActionFilter(ILogger<ReponseHeaderActionFilter> logger, string key, string value, int order)
        {
            _logger = logger;
            _key = key;
            _value = value;
            Order = order;

        }
        public void OnActionExecuting(ActionExecutingContext context)
        {

        }


        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("{ActionFilterName}-{MethodName} method", nameof(ReponseHeaderActionFilter), nameof(OnActionExecuted));
            context.HttpContext.Response.Headers[_key] = _value;
            _logger.LogInformation("header: {key} with value: {value} has been set", _key, _value);
        }

    }
}
