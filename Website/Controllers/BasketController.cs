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
}