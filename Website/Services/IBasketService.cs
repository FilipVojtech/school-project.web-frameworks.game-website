using Website.Models;

namespace Website.Services;

public interface IBasketService
{
    Task<int> ProductCount(HttpContext httpContext);

    Task Add(HttpContext httpContext, long gameId);

    /// <summary>
    /// Fetch items in currently in the basket.
    /// </summary>
    /// <param name="httpContext">HttpContext object</param>
    /// <returns>Promise that resolves in List of Basket Items.</returns>
    Task<IList<BasketItem>> Items(HttpContext httpContext);
}