#pragma warning disable CS8618
namespace WeddingPlanner.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Wedding
{
  [Key]
  public int WeddingId { get; set; }

  [Required(ErrorMessage = "Wedder One's name is required!")]
  [MinLength(4, ErrorMessage = "Wedder One's name must be included and at least 4 characters long!")]
  [Display(Name = "Wedder One")]
  public string WedderOne { get; set; }

  [Required(ErrorMessage = "Wedder Two's name is required!")]
  [MinLength(4, ErrorMessage = "Wedder Two's name must be included and at least 4 characters long!")]
  [Display(Name = "Wedder Two")]
  public string WedderTwo { get; set; }

  [Required]
  [Display(Name = "Wedding Address")]
  public string Address { get; set; }

  [Required]
  [DateValidator]
  public DateTime Date { get; set; }

  public DateTime createdAt { get; set; } = DateTime.Now;
  public DateTime updatedAt { get; set; } = DateTime.Now;

  public int UserId { get; set; }

  public List<User> Guests { get; set; } = new List<User>();

}