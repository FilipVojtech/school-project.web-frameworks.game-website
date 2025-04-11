using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Website.Data;
using Website.Models.ViewModels;
using Game = Website.Models.Game;

namespace Website.Controllers;

[Route("[controller]")]
public class GamesController(ApplicationDbContext context) : Controller
{
    private readonly ApplicationDbContext _context = context;

    // GET
    [HttpGet]
    public async Task<IActionResult> Index(string? query)
    {
        if (string.IsNullOrEmpty(query))
        {
            var model = new GamesViewModel
            {
                Games = await _context.Games
                    .AsNoTracking()
                    .OrderBy(g => g.Name)
                    .Include(g => g.Genres)
                    .Take(20)
                    .ToListAsync()
            };

            return View(model);
        }
        else
        {
            var model = new SearchGamesViewModel();
            ICollection<Game> games = await _context.Games
                .AsNoTracking()
                .Where(g => EF.Functions.Like(g.Name, $"%{query}%"))
                .Include(g => g.Genres)
                .OrderBy(g => g.Name)
                .Take(20)
                .ToListAsync();

            model.Query = query;
            model.SearchResults = games;

            return View("SearchGames", model);
        }
    }

    // GET: /Games/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> ViewGame(int? id)
    {
        Console.WriteLine($"Game ID: {id}");

        var game = await _context.Games
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.Id == id);

        if (game == null)
        {
            return NotFound("Could not find this game");
        }

        return View(game);
    }
}