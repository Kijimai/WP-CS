#pragma warning disable CS8618
namespace WeddingPlanner.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Wedding
{
  [Key]
  public int WeddingId { get; set; }

  [Required]
  [Display(Name = "Wedder One")]
  public string WedderOne { get; set; }

  [Required]
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

  public List<User> GuestCount { get; set; }

}