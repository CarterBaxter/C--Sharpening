using System.ComponentModel.DataAnnotations;

namespace TestApp1.Validation;

// A reusable validation attribute: put [NotInFuture] on any DateTime property.
public class NotInFutureAttribute : ValidationAttribute
{
    public NotInFutureAttribute()
        : base("{0} can't be in the future.")
    {
    }

    public override bool IsValid(object? value)
    {
        // Leave missing values to [Required]
        return value is not DateTime date || date.Date <= DateTime.Today;
    }
}
