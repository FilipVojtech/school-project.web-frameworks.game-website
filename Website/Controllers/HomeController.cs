using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Website.Data;
using Website.Helpers;
using Website.Models;
using Website.Models.ViewModels;

namespace Website.Controllers;

[Controller]
public class HomeController(
    ApplicationDbContext context,
    RoleManager<IdentityRole> roleManager,
    UserManager<User> userManager) : Controller
{
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;

    private readonly UserManager<User> _userManager = userManager;

    [HttpGet]
    public async Task<IActionResult> Index(int pageNumber = 1)
    {
        var reviews = context.Reviews
            .AsNoTracking()
            .OrderBy(r => r.CreatedAt)
            .Include(r => r.Game)
            .Include(r => r.Author);

        var homeVm = new HomeViewModel
        {
            Reviews = await PaginatedList<Review>.CreateAsync(reviews, pageNumber, 50),
        };

        return View(homeVm);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public async Task<IActionResult> CreateRolesAndUser()
    {
        const string adminUserEmail = "";
        var adminExists = await _roleManager.RoleExistsAsync("Admin");
        if (!adminExists)
        {
            await _roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        var user = await _userManager.FindByEmailAsync(adminUserEmail);
        if (user != null)
        {
            if (!await _userManager.IsInRoleAsync(user, "Admin"))
            {
                await _userManager.AddToRoleAsync(user, "Admin");
            }
        }

        return RedirectToAction(nameof(Index));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}