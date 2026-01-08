using AspNetSecurityApplication.Data;
using AspNetSecurityApplication.Models;
using AspNetSecurityApplication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetSecurityApplication.Controllers;

public class AuthController : Controller
{
    private readonly AppDbContext _db;
    private readonly IJwtService _jwt;

    public AuthController(AppDbContext db, IJwtService jwt)
    {
        _db = db;
        _jwt = jwt;
    }

    [AllowAnonymous]
    public IActionResult Login() => View(new AuthRequest());

    [AllowAnonymous]
    public IActionResult Register() => View(new AuthRequest { IsRegister = true });

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register(AuthRequest model)
    {
        model.IsRegister = true;

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (await _db.Users.AnyAsync(x => x.Email == model.Email))
        {
            ModelState.AddModelError(string.Empty, "Email already exists");
            return View(model);
        }

        var user = new ApplicationUser
        {
            FullName = model.FullName,
            Email = model.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password)
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        TempData["Message"] = "Account created. Please login.";
        return RedirectToAction("Login");
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(AuthRequest model)
    {
        model.IsRegister = false;

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _db.Users.FirstOrDefaultAsync(x => x.Email == model.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
        {
            ModelState.AddModelError(string.Empty, "Invalid email or password");
            return View(model);
        }

        var token = _jwt.GenerateToken(user);
        SetAuthCookie(token);

        return RedirectToAction("Index", "Users");
    }

    [Authorize]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("AuthToken");
        return RedirectToAction("Login");
    }

    private void SetAuthCookie(string token)
    {
        Response.Cookies.Append("AuthToken", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = Request.IsHttps,
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.UtcNow.AddMinutes(2)
        });
    }
}
