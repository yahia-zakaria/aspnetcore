using System.ComponentModel.DataAnnotations;

namespace aspnetcore.CustomValidations
{
    public class DateRangeAttribute : ValidationAttribute
    {
        public string OtherPropertyName { get; private set; }
        public string DefaultErrorMessage { get; private set; } = "{1} can not be after {0}";
        public DateRangeAttribute(string otherPropertyName)
        {
            OtherPropertyName = otherPropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value is not null)
            {
                var startDateProperty = validationContext.ObjectType.GetProperty(OtherPropertyName); 
                if(startDateProperty is not null)
                {
                    var startDatePropertyValue = startDateProperty.GetValue(validationContext.ObjectInstance);
                    DateTime startDate = Convert.ToDateTime(startDatePropertyValue);
                    DateTime endDate = (DateTime)value;
                    if (startDate > endDate)
                    {
                        var StartDateDisplayName = startDateProperty.GetCustomAttributes(false)
                            .ToDictionary(a => a.GetType().Name, a => a)
                            .TryGetValue("DisplayAttribute", out object Display) ? (Display as DisplayAttribute).Name : string.Empty;
                        return new ValidationResult(string.Format(ErrorMessage ?? DefaultErrorMessage, validationContext.DisplayName, StartDateDisplayName));
                    }
                    else
                    {
                        return ValidationResult.Success;
                    }
                }
            }
            return null;
        }
    }
}
