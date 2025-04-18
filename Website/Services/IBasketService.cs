namespace Website.Services;

public interface IBasketService
{
    Task<int> ProductCount(HttpContext httpContext);

    Task Add(HttpContext httpContext, long gameId);
}