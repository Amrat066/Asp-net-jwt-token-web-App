using AspNetSecurityApplication.Data;
using AspNetSecurityApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetSecurityApplication.Controllers;

[Authorize]
public class UsersController : Controller
{
    private readonly AppDbContext _db;

    public UsersController(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _db.Users.AsNoTracking().ToListAsync());
    }

    public IActionResult Create() => View(new ApplicationUser());

    [HttpPost]
    public async Task<IActionResult> Create(ApplicationUser model)
    {
        if (string.IsNullOrWhiteSpace(model.Password))
        {
            ModelState.AddModelError(nameof(model.Password), "Password is required.");
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (await _db.Users.AnyAsync(u => u.Email == model.Email))
        {
            ModelState.AddModelError(nameof(model.Email), "Email is already in use.");
            return View(model);
        }

        model.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password!);
        model.Password = null;

        _db.Users.Add(model);
        await _db.SaveChangesAsync();
        TempData["Message"] = "User created.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
        if (user == null) return NotFound();

        return View(user);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ApplicationUser model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == model.Id);
        if (user == null) return NotFound();

        if (await _db.Users.AnyAsync(u => u.Email == model.Email && u.Id != model.Id))
        {
            ModelState.AddModelError(nameof(model.Email), "Email is already in use.");
            return View(model);
        }

        user.FullName = model.FullName;
        user.Email = model.Email;

        if (!string.IsNullOrWhiteSpace(model.Password))
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);
        }
        model.Password = null;

        await _db.SaveChangesAsync();
        TempData["Message"] = "User updated.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
        if (user == null) return NotFound();
        return View(user);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user == null) return NotFound();

        _db.Users.Remove(user);
        await _db.SaveChangesAsync();
        TempData["Message"] = "User deleted.";
        return RedirectToAction(nameof(Index));
    }
}
