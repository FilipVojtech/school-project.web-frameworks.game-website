using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Website.Data;
using Website.Helpers;
using Website.Models;

namespace Website.Controllers;

[Authorize]
[Route("Reviews")]
public class ReviewsController(ApplicationDbContext context, UserManager<User> userManager) : Controller
{
    private readonly ApplicationDbContext _context = context;

    private readonly UserManager<User> _userManager = userManager;

    // GET
    public async Task<IActionResult> Index(int pageNumber = 1)
    {
        var user = (await _userManager.GetUserAsync(User))!;
        var reviews = _context.Reviews
            .Include(r => r.Author)
            .Include(r => r.Game)
            .Where(r => r.Author == user)
            .AsNoTracking();
        var list = await PaginatedList<Review>.CreateAsync(reviews, pageNumber, 20);
        return View(list);
    }
}