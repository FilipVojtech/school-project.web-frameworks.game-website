using IGDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Website.Data;
using Website.Models.ViewModels;
using Game = Website.Models.Game;

namespace Website.Controllers;

[Route("[controller]")]
public class GamesController(IGDBClient igdb, ApplicationDbContext context) : Controller
{
    private readonly IGDBClient _igdb = igdb;

    private readonly ApplicationDbContext _context = context;

    // GET
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var model = new GamesViewModel();

        return View(model);
    }

    public async Task<IActionResult> SearchGames(string query)
    {
        var model = new SearchGamesViewModel();
        ICollection<Game> games = await _context.Games
            .AsNoTracking()
            .Where(g => g.Name.Contains(query))
            .Take(20)
            .ToListAsync();

        model.Query = query;
        model.SearchResults = games;

        return View(model);
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