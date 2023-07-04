using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Filters;

namespace aspnetcore.Filters.ActionFilters
{
    public class ReponseHeaderActionFilter : IAsyncActionFilter, IOrderedFilter
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

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _logger.LogInformation("{ActionFilterName}-{MethodName} method - befor", nameof(ReponseHeaderActionFilter), nameof(OnActionExecutionAsync));

            await next();

            _logger.LogInformation("{ActionFilterName}-{MethodName} method - after", nameof(ReponseHeaderActionFilter), nameof(OnActionExecutionAsync));
            context.HttpContext.Response.Headers[_key] = _value;
            _logger.LogInformation("header: {key} with value: {value} has been set", _key, _value);
        }
    }
}
