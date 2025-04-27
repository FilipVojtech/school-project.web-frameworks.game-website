using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Website.Data;
using Website.Models;

namespace Website.Controllers;

[Authorize(Roles = "Admin")]
public class PlatformsAdminController(ApplicationDbContext context) : Controller
{
    // GET: PlatformsAdmin
    public async Task<IActionResult> Index()
    {
        return View(await context.Platforms.ToListAsync());
    }

    // GET: PlatformsAdmin/Details/5
    public async Task<IActionResult> Details(long? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var platform = await context.Platforms
            .FirstOrDefaultAsync(m => m.Id == id);
        if (platform == null)
        {
            return NotFound();
        }

        return View(platform);
    }

    // GET: PlatformsAdmin/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: PlatformsAdmin/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name")] Platform platform)
    {
        if (ModelState.IsValid)
        {
            context.Add(platform);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(platform);
    }

    // GET: PlatformsAdmin/Edit/5
    public async Task<IActionResult> Edit(long? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var platform = await context.Platforms.FindAsync(id);
        if (platform == null)
        {
            return NotFound();
        }

        return View(platform);
    }

    // POST: PlatformsAdmin/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(long id, [Bind("Id,Name")] Platform platform)
    {
        if (id != platform.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                context.Update(platform);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlatformExists(platform.Id))
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

        return View(platform);
    }

    // GET: PlatformsAdmin/Delete/5
    public async Task<IActionResult> Delete(long? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var platform = await context.Platforms
            .FirstOrDefaultAsync(m => m.Id == id);
        if (platform == null)
        {
            return NotFound();
        }

        return View(platform);
    }

    // POST: PlatformsAdmin/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(long id)
    {
        var platform = await context.Platforms.FindAsync(id);
        if (platform != null)
        {
            context.Platforms.Remove(platform);
        }

        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool PlatformExists(long id)
    {
        return context.Platforms.Any(e => e.Id == id);
    }
}