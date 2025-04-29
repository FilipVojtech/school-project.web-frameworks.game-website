using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Website.Data;
using Website.Helpers;
using Website.Models;
using Website.Models.ViewModels;

namespace Website.Controllers;

[Route("[controller]")]
public class GamesController(ApplicationDbContext context, UserManager<User> userManager) : Controller
{
    private readonly ApplicationDbContext _context = context;

    private readonly UserManager<User> _userManager = userManager;

    // GET
    [HttpGet]
    public async Task<IActionResult> Index(
        string? query,
        string sortOrder,
        string currentFilter,
        int? pageNumber,
        long? genreId)
    {
        ViewData["CurrentSort"] = sortOrder;
        ViewData["Query"] = query;
        ViewData["SelectedGenre"] = genreId;

        if (query != null)
        {
            pageNumber = 1;
        }
        else
        {
            query = currentFilter;
        }

        ViewData["CurrentFilter"] = query;

        IQueryable<Game> games = _context.Games
            .AsNoTracking()
            .Include(g => g.Genres);

        if (!string.IsNullOrEmpty(query))
        {
            games = games.Where(g => EF.Functions.Like(g.Name, $"%{query}%"));
        }

        if (genreId != null)
        {
            games = games.Where(g => g.Genres.Any(ge => ge.Id == genreId));
        }

        games = sortOrder switch
        {
            "name_desc" => games.OrderByDescending(g => g.Name),
            "price" => games.OrderBy(g => g.Price),
            "price_desc" => games.OrderByDescending(g => g.Price),
            _ => games.OrderBy(g => g.Name)
        };

        const int pageSize = 20;

        return View(await PaginatedList<Game>.CreateAsync(games, pageNumber ?? 1, pageSize));
    }

    // GET: /Games/{id}
    [HttpGet("{id:long}")]
    public async Task<IActionResult> ViewGame(long id, AddReviewModel? addReviewModel = null, int pageNumber = 1)
    {
        var model = new ViewGame();
        var game = await _context.Games
            .AsNoTracking()
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

        var reviews = _context.Reviews
            .AsNoTracking()
            .Include(r => r.Author)
            .Where(r => r.Game == game)
            .OrderByDescending(r => r.CreatedAt);

        model.ReviewCount = await reviews.CountAsync();
        if (model.ReviewCount > 0)
            model.AverageRating = await reviews.AverageAsync(r => r.Rating);

        model.Reviews = await PaginatedList<Review>.CreateAsync(reviews, pageNumber, 20);

        return View(model);
    }

    // GET: /Games/{id}
    [HttpPost("{id:int}")]
    public async Task<IActionResult> PostReview(int id, AddReviewModel model)
    {
        var author = await _userManager.GetUserAsync(User);
        if (author == null)
        {
            Unauthorized("Please log in to write reviews");
        }

        var game = await _context.Games.FirstOrDefaultAsync(g => g.Id == id);

        if (game == null)
        {
            return NotFound("Could not find this game");
        }

        if (!ModelState.IsValid)
        {
            return RedirectToAction(nameof(ViewGame), new { id = id, addReviewModel = model });
        }

        game.Reviews.Add(new Review
        {
            Author = author,
            Title = model.Title,
            Body = model.Body,
            Rating = model.Rating,
            Game = game
        });
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(ViewGame), new { id = id });
    }
}