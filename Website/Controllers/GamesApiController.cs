using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Website.Data;
using Website.Helpers;
using Website.Models;

namespace Website.Controllers;

[Route("api/Games")]
[ApiController]
public class GamesApiController(ApplicationDbContext context) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(int page = 1)
    {
        var games = context.Games
            .Include(g => g.Genres)
            .Include(g => g.Platforms)
            .AsNoTracking();
        var results = await PaginatedList<Game>.CreateAsync(games, page, 20);
        return Ok(results.ToList());
    }
}