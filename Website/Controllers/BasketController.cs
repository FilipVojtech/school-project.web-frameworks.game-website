using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Website.Models.ViewModels;
using Website.Services;

namespace Website.Controllers;

[Authorize]
[Route("[controller]")]
public class BasketController(IBasketService basketService) : Controller
{
    private readonly IBasketService _basket = basketService;

    // GET
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var model = new ViewBasketViewModel
        {
            Items = await _basket.Items(),
            TotalPrice = await _basket.TotalPrice()
        };
        return View(model);
    }

    [HttpPost("AddToBasket")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddToBasket(int gameId)
    {
        var returnUrl = Request.GetTypedHeaders().Referer?.PathAndQuery ?? "/";
        await _basket.Add(gameId);
        return Redirect(returnUrl);
    }

    [HttpPost("SetQuantity")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SetQuantity(int gameId, int quantity)
    {
        var returnUrl = Request.GetTypedHeaders().Referer?.PathAndQuery ?? "/";
        await _basket.SetQuantity(gameId, quantity);
        return Redirect(returnUrl);
    }

    [HttpGet("Checkout")]
    public async Task<IActionResult> Checkout()
    {
        return View();
    }
}