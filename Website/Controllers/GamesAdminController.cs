using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Website.Data;
using Website.Models;

namespace Website.Controllers;

[Authorize(Roles = "Admin")]
public class GamesAdminController(ApplicationDbContext context) : Controller
{
    // GET: GamesAdmin
    public async Task<IActionResult> Index()
    {
        return View(await context.Games.ToListAsync());
    }

    // GET: GamesAdmin/Details/5
    public async Task<IActionResult> Details(long? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var game = await context.Games
            .Include(g => g.Genres)
            .Include(g => g.Platforms)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (game == null)
        {
            return NotFound();
        }

        return View(game);
    }

    // GET: GamesAdmin/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: GamesAdmin/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Id,Name,Url,ImageUrl,ReleaseDate,Developer,Publisher,Description,Price,Public")]
        Game game)
    {
        if (ModelState.IsValid)
        {
            context.Add(game);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(game);
    }

    // GET: GamesAdmin/Edit/5
    public async Task<IActionResult> Edit(long? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var game = await context.Games
            .Include(g => g.Genres)
            .Include(g => g.Platforms)
            .FirstOrDefaultAsync(g => g.Id == id);
        if (game == null)
        {
            return NotFound();
        }

        ViewData["Genres"] = await context.Genres
            .AsNoTracking()
            .ToListAsync();
        ViewData["Platforms"] = await context.Platforms
            .AsNoTracking()
            .ToListAsync();

        return View(game);
    }

    // POST: GamesAdmin/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(long id,
        List<string> genres,
        IList<string> platforms,
        [Bind("Id,Name,Url,ImageUrl,ReleaseDate,Developer,Publisher,Description,Price,Public")]
        Game game)
    {
        if (id != game.Id)
        {
            return NotFound();
        }

        var genreObjects = await context.Genres
            .Where(g => genres.Contains(g.Id.ToString()))
            .ToListAsync();
        var platformObjects = await context.Platforms
            .Where(p => platforms.Contains(p.Id.ToString()))
            .ToListAsync();

        if (ModelState.IsValid)
        {
            try
            {
                context.Update(game);
                game = await context.Games
                    .Include(g => g.Genres)
                    .Include(g => g.Platforms)
                    .FirstAsync(g => g.Id == game.Id);
                game.Genres = genreObjects;
                game.Platforms = platformObjects;
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameExists(game.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(Index));
        }

        return View(game);
    }

    // GET: GamesAdmin/Delete/5
    public async Task<IActionResult> Delete(long? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var game = await context.Games
            .FirstOrDefaultAsync(m => m.Id == id);
        if (game == null)
        {
            return NotFound();
        }

        return View(game);
    }

    // POST: GamesAdmin/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(long id)
    {
        var game = await context.Games.FindAsync(id);
        if (game != null)
        {
            context.Games.Remove(game);
        }

        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool GameExists(long id)
    {
        return context.Games.Any(e => e.Id == id);
    }
}