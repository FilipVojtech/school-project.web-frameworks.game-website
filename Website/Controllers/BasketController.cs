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
    public async Task<IActionResult> Index()
    {
        var model = new ViewBasketViewModel
        {
            Items = await _basket.Items(),
            TotalPrice = await _basket.TotalPrice()
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> AddToBasket(int gameId)
    {
        var returnUrl = Request.GetTypedHeaders().Referer?.PathAndQuery ?? "/";
        await _basket.Add(gameId);
        return Redirect(returnUrl);
    }
}