#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


[NotMapped]
public class LoginUser
{
  [Required(ErrorMessage = "Is Required.")]
  [EmailAddress]
  public string LoginEmail { get; set; }

  [Required(ErrorMessage = "Is Required.")]
  [MinLength(8, ErrorMessage = "Must be at least 8 characters long.")]
  [DataType(DataType.Password)]
  public string LoginPassword { get; set; }
}