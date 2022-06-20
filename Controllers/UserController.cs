using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WeddingPlanner.Models;
using Microsoft.AspNetCore.Identity;

namespace WeddingPlanner.Controllers;

public class UserController : Controller
{
  private WeddingPlannerContext _context;

  public UserController(WeddingPlannerContext context)
  {
    _context = context;
  }
  [HttpGet("")]
  public IActionResult LoginAndRegister()
  {
    return View("LoginAndRegister");
  }

  // [HttpGet("dashboard")]
  // public IActionResult Dashboard() {

  // } 

  [HttpPost("register/new")]
  public IActionResult RegisterUser(User newUser)
  {
    if (ModelState.IsValid == false)
    {
      return LoginAndRegister();
    }

    if (_context.Users.Any(user => user.Email == newUser.Email))
    {
      ModelState.AddModelError("Email", "is already taken!");
      return LoginAndRegister();
    }

    PasswordHasher<User> Hasher = new PasswordHasher<User>();
    newUser.Password = Hasher.HashPassword(newUser, newUser.Password);

    

    _context.Users.Add(newUser);
    _context.SaveChanges();
    return RedirectToAction("Dashboard");
  }
}