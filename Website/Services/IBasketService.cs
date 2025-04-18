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
    /// Set quantity of a game in the basket. Removed the game from the basket if quantity is zero.
    /// </summary>
    /// <param name="gameId">The ID of the game which quantity should be updated.</param>
    /// <param name="quantity">The new quantity.</param>
    Task SetQuantity(int gameId, int quantity);

    /// <summary>
    /// Fetch items in currently in the basket.
    /// </summary>
    /// <returns>Promise that resolves in List of Basket Items.</returns>
    Task<IList<BasketItem>> Items();

    /// <summary>
    /// Calculate the total price of the basket
    /// </summary>
    /// <returns>Total price of the basket or zero if no basket is assigned to the user</returns>
    Task<decimal> TotalPrice();
}