using Microsoft.AspNetCore.Mvc.Filters;

namespace aspnetcore.Filters.SkipFilters
{
    public class SkipAlwaysRunResultFilter : Attribute, IFilterMetadata
    {
    }
}
