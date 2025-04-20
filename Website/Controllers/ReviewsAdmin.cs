using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Website.Data;
using Website.Models;

namespace Website.Controllers;

public class ReviewsAdmin(ApplicationDbContext context) : Controller
{
    // GET: ReviewsAdmin
    public async Task<IActionResult> Index()
    {
        return View(await context.Reviews.ToListAsync());
    }

    // GET: ReviewsAdmin/Details/5
    public async Task<IActionResult> Details(long? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var review = await context.Reviews
            .FirstOrDefaultAsync(m => m.Id == id);
        if (review == null)
        {
            return NotFound();
        }

        return View(review);
    }

    // GET: ReviewsAdmin/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: ReviewsAdmin/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Title,Body,Rating")] Review review)
    {
        if (ModelState.IsValid)
        {
            context.Add(review);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(review);
    }

    // GET: ReviewsAdmin/Edit/5
    public async Task<IActionResult> Edit(long? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var review = await context.Reviews.FindAsync(id);
        if (review == null)
        {
            return NotFound();
        }
        return View(review);
    }

    // POST: ReviewsAdmin/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(long id, [Bind("Id,Title,Body,Rating")] Review review)
    {
        if (id != review.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                context.Update(review);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(review.Id))
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