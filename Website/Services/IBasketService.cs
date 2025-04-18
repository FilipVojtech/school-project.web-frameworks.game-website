using Website.Models;

namespace Website.Services;

public interface IBasketService
{
    /// <summary>
    /// Get the count of products in the basket.
    /// </summary>
    /// <returns></returns>
    Task<int> ProductCount();

    /// <summary>
    /// Add a game to a basket. Or increase its count.
    /// </summary>
    /// <param name="gameId">ID of the game to be added</param>
    /// <returns></returns>
    Task Add(long gameId);

    /// <summary>
    /// Fetch items in currently in the basket.
    /// </summary>
    /// <returns>Promise that resolves in List of Basket Items.</returns>
    Task<IList<BasketItem>> Items();
}