using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Website.Data;
using Website.Models;
using Website.Models.ViewModels;

namespace Website.Controllers;

[Route("[controller]")]
public class GamesController(
    ApplicationDbContext context,
    ILogger<GamesController> logger,
    UserManager<User> userManager) : Controller
{
    private readonly ApplicationDbContext _context = context;

    // GET
    [HttpGet]
    public async Task<IActionResult> Index(string? query)
    {
        var model = new GamesViewModel { Query = query };
        if (string.IsNullOrEmpty(query))
        {
            model.Games = await _context.Games
                .AsNoTracking()
                .OrderBy(g => g.Name)
                .Include(g => g.Genres)
                .Take(20)
                .ToListAsync();
        }
        else
        {
            model.Games = await _context.Games
                .AsNoTracking()
                .Where(g => EF.Functions.Like(g.Name, $"%{query}%"))
                .Include(g => g.Genres)
                .OrderBy(g => g.Name)
                .Take(20)
                .ToListAsync();
        }

        return View(model);
    }

    // GET: /Games/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> ViewGame(int id, AddReviewModel? addReviewModel = null)
    {
        var model = new ViewGame();
        var game = await _context.Games
            .AsNoTracking()
            .Include(g => g.Reviews)
            .ThenInclude(g => g.Author)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (game == null)
        {
            return NotFound("Could not find this game");
        }

        model.Game = game;
        model.AddReviewModel.GameId = game.Id;

        if (addReviewModel != null && addReviewModel.GameId != 0)
        {
            model.AddReviewModel = addReviewModel;
        }

        return View(model);
    }
}