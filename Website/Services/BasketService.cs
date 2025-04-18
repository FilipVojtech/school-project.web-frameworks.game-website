using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Website.Data;
using Website.Models;

namespace Website.Services;

public class BasketService(
    ApplicationDbContext context,
    UserManager<User> userManager,
    IHttpContextAccessor httpContextAccessor)
    : IBasketService
{
    private readonly ApplicationDbContext _context = context;

    private readonly UserManager<User> _userManager = userManager;

    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    private async Task<Basket?> GetOrCreateBasket()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            return null;
        }

        var user = await _userManager.GetUserAsync(httpContext.User);
        if (user == null)
        {
            return null;
        }

        var basket = await _context.Baskets
            .Include(b => b.Items)
            .ThenInclude(i => i.Game)
            .FirstOrDefaultAsync(b => b.UserId == user.Id);

        if (basket == null)
        {
            basket = new Basket { UserId = user.Id };
            await _context.Baskets.AddAsync(basket);
            if (await _context.SaveChangesAsync() == 0)
            {
                return null;
            }
        }

        return basket;
    }

    public async Task<int> ProductCount()
    {
        var basket = await GetOrCreateBasket();
        return basket == null ? 0 : basket.Items.Count;
    }

    public async Task Add(long gameId)
    {
        var basket = await GetOrCreateBasket();
        if (basket == null) return;

        var game = await _context.Games
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.Id == gameId);
        if (game == null)
        {
            return;
        }

        var item = basket.Items.FirstOrDefault(i => i.GameId == gameId);
        if (item != null)
        {
            item.Count++;
        }
        else
        {
            basket.Items.Add(new BasketItem
            {
                Game = game,
                Count = 1,
            });
        }

        await _context.SaveChangesAsync();
    }

    public async Task SetQuantity(int gameId, int quantity)
    {
        var basket = await GetOrCreateBasket();
        if (basket == null) return;

        var item = basket.Items.FirstOrDefault(i => i.GameId == gameId);
        if (item == null) return;

        if (quantity == 0)
        {
            basket.Items.Remove(item);
        }
        else
        {
            item.Count = quantity;
        }

        await _context.SaveChangesAsync();
    }

    public async Task<IList<BasketItem>> Items()
    {
        IList<BasketItem> items = new List<BasketItem>();
        var basket = await GetOrCreateBasket();
        if (basket != null)
        {
            items = basket.Items;
        }

        return items;
    }

    public async Task<decimal> TotalPrice()
    {
        var basket = await GetOrCreateBasket();
        return basket?.TotalPrice() ?? 0;
    }
}