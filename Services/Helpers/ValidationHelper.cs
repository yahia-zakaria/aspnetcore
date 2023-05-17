

using System.ComponentModel.DataAnnotations;

namespace Services.Helpers
{
    public class ValidationHelper
    {
        public static void Validate(object obj)
        {
            ValidationContext validationContext = new ValidationContext(obj);
            List<ValidationResult> validationResults = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);

            if(isValid == false)
            {
                throw new ArgumentException(string.Join("\\n", validationResults.Select(err => err.ErrorMessage)));
            }

        }
    }
}
