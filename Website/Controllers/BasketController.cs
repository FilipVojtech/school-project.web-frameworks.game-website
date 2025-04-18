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
    public IActionResult Index()
    {
        var model = new ViewBasketViewModel();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> AddToBasket(int gameId)
    {
        var returnUrl = Request.GetTypedHeaders().Referer?.PathAndQuery ?? "/";
        await _basketService.Add(HttpContext, gameId);
        return Redirect(returnUrl);
    }
}