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
    private bool ReviewExists(long reviewId)
    {
        return _context.Reviews.Any(r => r.Id == reviewId);
    }

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

    [Route("Edit")]
    public async Task<IActionResult> Edit(int reviewId)
    {
        var returnUrl = Request.GetTypedHeaders().Referer?.PathAndQuery ?? "/";
        var review = await _context.Reviews.FirstOrDefaultAsync(r => r.Id == reviewId);
        ViewData["ReturnUrl"] = returnUrl;

        if (review == null)
        {
            return NotFound();
        }

        return View(review);
    }

    [HttpPost("Edit")]
    public async Task<IActionResult> Edit(string returnUrl, long id,
        [Bind("Id,Title,Body,Rating,AuthorId,GameId")]
        Review review)
    {
        if (id != review.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(review);
                await _context.SaveChangesAsync();
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

            return Redirect(returnUrl);
        }

        ViewData["ReturnUrl"] = returnUrl;
        return View(review);
    }

    [Route("Delete")]
    public async Task<IActionResult> Delete(long? id)
    {
        var returnUrl = Request.GetTypedHeaders().Referer?.PathAndQuery ?? "/";
        ViewData["ReturnUrl"] = returnUrl;

        if (id == null)
        {
            return NotFound();
        }

        var review = await _context.Reviews
            .Include(r => r.Author)
            .Include(r => r.Game)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (review == null)
        {
            return NotFound();
        }


        return View(review);
    }

    [HttpPost("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string returnUrl, long id)
    {
        var review = await context.Reviews.FindAsync(id);
        if (review != null)
        {
            context.Reviews.Remove(review);
        }
        await context.SaveChangesAsync();

        return Redirect(returnUrl);
    }
}