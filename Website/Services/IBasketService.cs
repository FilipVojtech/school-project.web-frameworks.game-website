using Website.Models;

namespace Website.Services;

public interface IBasketService
{
    /// <summary>
    /// Get the count of products in the basket.
    /// </summary>
    /// <param name="httpContext">HttpContext object</param>
    /// <returns></returns>
    Task<int> ProductCount(HttpContext httpContext);

    /// <summary>
    /// Add a game to a basket. Or increase its count.
    /// </summary>
    /// <param name="httpContext">HttpContext object</param>
    /// <param name="gameId">ID of the game to be added</param>
    /// <returns></returns>
    Task Add(HttpContext httpContext, long gameId);

    /// <summary>
    /// Fetch items in currently in the basket.
    /// </summary>
    /// <param name="httpContext">HttpContext object</param>
    /// <returns>Promise that resolves in List of Basket Items.</returns>
    Task<IList<BasketItem>> Items(HttpContext httpContext);
}