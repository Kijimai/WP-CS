using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WeddingPlanner.Models;
using Microsoft.AspNetCore.Identity;

namespace WeddingPlanner.Controllers;

public class WeddingController : Controller
{
  private int? userId
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
      return userId != null;
    }
  }

  private WeddingPlannerContext _context;

  public WeddingController(WeddingPlannerContext context)
  {
    _context = context;
  }

  [HttpGet("weddings/create")]
  public IActionResult Create()
  {
    return View("CreateWedding");
  }

  [HttpPost("weddings/new")]
  public IActionResult New(Wedding newWedding)
  {
    if (ModelState.IsValid == false)
    {
      return RedirectToAction("Create");
    }
    _context.Weddings.Add(newWedding);
    _context.SaveChanges();
    return RedirectToAction("Dashboard", "User");
  }

}