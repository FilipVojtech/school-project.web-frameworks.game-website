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

    private Basket? _basket;

    public Basket? Basket
    {
        get
        {
            if (_basket != null)
            {
                return _basket;
            }

            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null) return null;
            var userId = _userManager.GetUserId(httpContext.User);
            if (userId == null) return null;

            var basket = _context.Baskets
                .Include(b => b.Items)
                .ThenInclude(i => i.Game)
                .FirstOrDefault(b => b.UserId == userId);

            if (basket == null)
            {
                basket = new Basket { UserId = userId };
                _context.Baskets.Add(basket);
                _context.SaveChanges();
            }

            return basket;
        }
        private set => _basket = value;
    }

    public int ProductCount => Basket == null ? 0 : Basket.Items.Count;

    public decimal TotalPrice => Basket?.TotalPrice() ?? 0;

    public IList<BasketItem> Items => Basket?.Items ?? [];

    public async Task Add(long gameId)
    {
        if (Basket == null) return;

        var game = await _context.Games
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.Id == gameId);
        if (game == null)
        {
            return;
        }

        var item = Basket.Items.FirstOrDefault(i => i.GameId == gameId);
        if (item != null)
        {
            item.Count++;
        }
        else
        {
            Basket.Items.Add(new BasketItem
            {
                Game = game,
                Count = 1,
            });
        }

        await _context.SaveChangesAsync();
    }

    public async Task SetQuantity(int gameId, int quantity)
    {
        if (Basket == null) return;

        var item = Basket.Items.FirstOrDefault(i => i.GameId == gameId);
        if (item == null) return;

        if (quantity == 0)
        {
            Basket.Items.Remove(item);
        }
        else
        {
            item.Count = quantity;
        }

        await _context.SaveChangesAsync();
    }

    public async Task CloseBasket()
    {
        await EmptyBasket();
    }

    public async Task EmptyBasket()
    {
        if (Basket == null)
        {
            return;
        }

        _context.Attach(Basket);
        _context.Items.RemoveRange(Basket.Items);
        await _context.SaveChangesAsync();
    }
}