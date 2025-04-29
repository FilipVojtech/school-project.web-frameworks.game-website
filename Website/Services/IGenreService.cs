using Website.Models;

namespace Website.Services;

public interface IGenreService
{
    /// <summary>
    /// Get a list of all genres.
    /// </summary>
    /// <returns>List of all genres</returns>
    Task<IList<Genre>> All();
}