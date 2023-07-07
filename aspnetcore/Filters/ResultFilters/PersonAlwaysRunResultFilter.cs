using Microsoft.AspNetCore.Mvc.Filters;

namespace aspnetcore.Filters.ResultFilters
{
    public class PersonAlwaysRunResultFilter : IAlwaysRunResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext context)
        {
            throw new NotImplementedException();
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            throw new NotImplementedException();
        }
    }
}
