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
    int? loginUserID = HttpContext.Session.GetInt32("UserId");
    if (loginUserID == null)
    {
      return LoginAndRegister();
    }
    return View("Dashboard");
  }

  [HttpPost("/logout")]
  public IActionResult Logout()
  {
    HttpContext.Session.Clear();
    return RedirectToAction("LoginAndRegister");
  }
}