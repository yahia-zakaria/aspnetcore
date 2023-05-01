using System.ComponentModel.DataAnnotations;

namespace aspnetcore.CustomValidations
{
    public class MinimumYearAttribute : ValidationAttribute
    {
        string defaultErrorMessage = "The {0} Year must be after the {1}";
        public int year { get; set; } = 2000;
        public MinimumYearAttribute() { }
        public MinimumYearAttribute(int year)
        {
            this.year = year;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is not null)
            {
                DateTime date = (DateTime)value;
                if (date.Year < year)
                {
                    return new ValidationResult(string.Format(ErrorMessage ?? defaultErrorMessage, validationContext.DisplayName, year));
                }
                else
                {
                    return ValidationResult.Success;
                }
            }

            return null;
        
        }
    }
}
