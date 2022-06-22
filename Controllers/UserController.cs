using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WeddingPlanner.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WeddingPlanner.Controllers;

public class UserController : Controller
{
  private WeddingPlannerContext _context;

  public UserController(WeddingPlannerContext context)
  {
    _context = context;
  }

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

  [HttpGet("")]
  public IActionResult LoginAndRegister()
  {
    return View("LoginAndRegister");
  }

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

    HttpContext.Session.SetInt32("UserId", newUser.UserId);

    return RedirectToAction("Dashboard");
  }

  [HttpPost("/login")]
  public IActionResult Login(LoginUser loginUser)
  {
    if (ModelState.IsValid == false)
    {
      ViewBag.AuthorizationError = "Your email or password is incorrect!";
      return LoginAndRegister();
    }

    User? foundUser = _context.Users.FirstOrDefault(u => u.Email == loginUser.LoginEmail);

    if (foundUser == null)
    {
      ModelState.AddModelError("Email", "and Password do not match.");
      return LoginAndRegister();
    }

    PasswordHasher<LoginUser> Hasher = new PasswordHasher<LoginUser>();
    PasswordVerificationResult comparePassword = Hasher.VerifyHashedPassword(loginUser, foundUser.Password, loginUser.LoginPassword);

    if (comparePassword == 0)
    {
      ModelState.AddModelError("Email", "and Password don't match!");
      return LoginAndRegister();
    }

    HttpContext.Session.SetInt32("UserId", foundUser.UserId);
    return RedirectToAction("Dashboard");
  }

  [HttpGet("dashboard")]
  public IActionResult Dashboard()
  {
    if (UserId == null)
    {
      return LoginAndRegister();
    }

    List<Wedding> AllWeddings = _context.Weddings.Include(wedding => wedding.Planner).Include(wedding => wedding.Guests).Where(wedding => wedding.Date > DateTime.Now).ToList();

    return View("Dashboard", AllWeddings);
  }

  [HttpPost("/logout")]
  public IActionResult Logout()
  {
    HttpContext.Session.Clear();
    return RedirectToAction("LoginAndRegister");
  }
}