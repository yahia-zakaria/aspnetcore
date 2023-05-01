

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace aspnetcore.Extensions
{
    public static class ModelStateExtensions
    {
        public static string GetModelErrors(this ModelStateDictionary modelState)
        {
            return string.Join(Environment.NewLine, modelState.Values.SelectMany(value=>value.Errors)
                .Select(err=>err.ErrorMessage));
        }
    }
}
