using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Website.Models.ViewModels;
using Website.Services;

namespace Website.Controllers;

[Authorize]
[Route("[controller]")]
public class BasketController(IBasketService basketService) : Controller
{
    private readonly IBasketService _basketService = basketService;

    // GET
    public async Task<IActionResult> Index()
    {
        var model = new ViewBasketViewModel
        {
            Items = await _basketService.Items()
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> AddToBasket(int gameId)
    {
        var returnUrl = Request.GetTypedHeaders().Referer?.PathAndQuery ?? "/";
        await _basketService.Add(gameId);
        return Redirect(returnUrl);
    }
}