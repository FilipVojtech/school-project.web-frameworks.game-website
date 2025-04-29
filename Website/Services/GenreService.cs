using Microsoft.EntityFrameworkCore;
using Website.Data;
using Website.Models;

namespace Website.Services;

public class GenreService(ApplicationDbContext context) : IGenreService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<IList<Genre>> All()
    {
        return await _context.Genres
            .AsNoTracking()
            .OrderBy(g => g.Name)
            .ToListAsync();
    }
}