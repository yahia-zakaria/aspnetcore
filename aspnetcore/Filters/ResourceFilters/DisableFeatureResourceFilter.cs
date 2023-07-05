using aspnetcore.Filters.ActionFilters;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace aspnetcore.Filters.ResourceFilters
{
    public class DisableFeatureResourceFilter : IAsyncResourceFilter
    {
        private readonly ILogger<DisableFeatureResourceFilter> _logger;
        private readonly bool _isDisabled;

        public DisableFeatureResourceFilter(ILogger<DisableFeatureResourceFilter> logger, bool isDisabled = true)
        {
            _logger = logger;

            _isDisabled = isDisabled;
        }

        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            if (_isDisabled)
            {
                _logger.LogInformation("{FilterName}-{MethodName} method - before", nameof(DisableFeatureResourceFilter), nameof(OnResourceExecutionAsync));
                context.Result = new StatusCodeResult(501);
            }
            else
            {
                await next();
                _logger.LogInformation("{FilterName}-{MethodName} method - after", nameof(DisableFeatureResourceFilter), nameof(OnResourceExecutionAsync));
            }
        }
    }
}
