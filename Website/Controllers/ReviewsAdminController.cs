using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Website.Data;

namespace Website.Controllers;

public class ReviewsAdminController(ApplicationDbContext context) : Controller
{
    // GET: ReviewsAdmin
    public async Task<IActionResult> Index()
    {
        return View(await context.Reviews.Include(r => r.Author).ToListAsync());
    }

    // GET: ReviewsAdmin/Details/5
    public async Task<IActionResult> Details(long? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var review = await context.Reviews
            .Include(r => r.Author)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (review == null)
        {
            return NotFound();
        }

        return View(review);
    }

    // GET: ReviewsAdmin/Delete/5
    public async Task<IActionResult> Delete(long? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var review = await context.Reviews
            .Include(r => r.Author)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (review == null)
        {
            return NotFound();
        }

        return View(review);
    }

    // POST: ReviewsAdmin/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(long id)
    {
        var review = await context.Reviews.FindAsync(id);
        if (review != null)
        {
            context.Reviews.Remove(review);
        }

        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ReviewExists(long id)
    {
        return context.Reviews.Any(e => e.Id == id);
    }
}