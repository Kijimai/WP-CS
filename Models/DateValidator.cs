using System.ComponentModel.DataAnnotations;

public class DateValidator : ValidationAttribute
{
  protected override ValidationResult IsValid(object value, ValidationContext context)
  {
    DateTime _dateOfWedding = Convert.ToDateTime(value);
    if (_dateOfWedding > DateTime.Now)
    {
      return ValidationResult.Success;
    }
    else
    {
      return new ValidationResult("Date of wedding can't be set in the past.");
    }
  }

}