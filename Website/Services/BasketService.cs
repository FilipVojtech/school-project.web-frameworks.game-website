using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Website.Data;
using Website.Models;

namespace Website.Services;

public class BasketService(ApplicationDbContext context, UserManager<User> userManager)
    : IBasketService
{
    private readonly ApplicationDbContext _context = context;

    private readonly UserManager<User> _userManager = userManager;

    private async Task<Basket?> GetOrCreateBasket(HttpContext httpContext)
    {
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

    public async Task<int> ProductCount(HttpContext httpContext)
    {
        var basket = await GetOrCreateBasket(httpContext);
        return basket == null ? 0 : basket.Items.Count;
    }

    public async Task Add(HttpContext httpContext, long gameId)
    {
        var basket = await GetOrCreateBasket(httpContext);
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
}