using aspnetcore.Filters.SkipFilters;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace aspnetcore.Filters.ResultFilters
{
    public class PersonAlwaysRunResultFilter : IAlwaysRunResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext context)
        {
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Filters.OfType<SkipAlwaysRunResultFilter>().Any())
            {
                return;
            }
        }
    }
}
