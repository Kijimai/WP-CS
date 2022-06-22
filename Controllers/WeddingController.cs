using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WeddingPlanner.Models;
using Microsoft.EntityFrameworkCore;

namespace WeddingPlanner.Controllers;

public class WeddingController : Controller
{
  private int? UserId
  {
    get
    {
      return HttpContext.Session.GetInt32("UserId");
    }
  }

  private bool loggedIn
  {
    get
    {
      return UserId != null;
    }
  }

  private WeddingPlannerContext _context;

  public WeddingController(WeddingPlannerContext context)
  {
    _context = context;
  }

  [HttpGet("/weddings/create")]
  public IActionResult Create()
  {
    if (!loggedIn)
    {
      return RedirectToAction("LoginAndRegister", "User");
    }
    return View("CreateWedding");
  }

  [HttpPost("/weddings/new")]
  public IActionResult New(Wedding newWedding)
  {
    if (UserId == null)
    {
      return RedirectToAction("LoginAndRegister", "User");
    }

    if (ModelState.IsValid == false)
    {
      return Create();
    }

    newWedding.UserId = (int)UserId;
    _context.Weddings.Add(newWedding);
    _context.SaveChanges();

    return RedirectToAction("Dashboard", "User");
  }

  [HttpGet("/weddings/{weddingId}")]
  public IActionResult ViewOneWedding(int weddingId)
  {
    if (!loggedIn)
    {
      return RedirectToAction("LoginAndRegister", "User");
    }

    Wedding? wedding = _context.Weddings.Include(wedding => wedding.Planner).Include(wedding => wedding.Guests).ThenInclude(attendee => attendee.Attendee).FirstOrDefault(wedding => wedding.WeddingId == weddingId);

    if (wedding == null)
    {
      return RedirectToAction("Dashboard", "User");
    }
    return View("SingleWedding", wedding);
  }

  [HttpPost("/weddings/{weddingId}/attend")]
  public IActionResult JoinWedding(int weddingId)
  {
    if (UserId == null)
    {
      return RedirectToAction("LoginAndRegister", "User");
    }

    Association? existingRSVP = _context.UserWeddingSignups.FirstOrDefault(rsvp => rsvp.UserId == UserId && rsvp.WeddingId == weddingId);

    if (existingRSVP == null)
    {
      Association newRSVP = new Association()
      {
        WeddingId = weddingId,
        UserId = (int)UserId
      };

      _context.UserWeddingSignups.Add(newRSVP);
    }
    else
    {
      _context.UserWeddingSignups.Remove(existingRSVP);
    }

    _context.SaveChanges();
    return RedirectToAction("Dashboard", "User");
  }
}