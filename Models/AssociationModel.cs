#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingPlanner.Models;

public class Association
{
  public int AssociationId { get; set; }

  public int UserId { get; set; }
  public User? Attendee { get; set; }

  public int WeddingId { get; set; }
  public Wedding? Wedding { get; set; }
}