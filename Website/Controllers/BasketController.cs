using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Website.Models;
using Website.Models.ViewModels;
using Website.Services;

namespace Website.Controllers;

[Authorize]
[Route("[controller]")]
public class BasketController(IBasketService basketService, UserManager<User> userManager) : Controller
{
    private readonly IBasketService _basket = basketService;

    private readonly UserManager<User> _userManager = userManager;

    // GET
    [HttpGet]
    public IActionResult Index()
    {
        var model = new ViewBasketViewModel
        {
            Items = _basket.Items,
            TotalPrice = _basket.TotalPrice
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
        var basket = _basket.Basket;
        if (basket == null)
        {
            return RedirectToAction(nameof(Index));
        }

        var emailAddress = await _userManager.GetEmailAsync((await _userManager.GetUserAsync(User))!);

        return View(new CheckoutViewModel(basket, emailAddress!));
    }

    [HttpPost("Checkout")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Checkout(CheckoutViewModel model)
    {
        model.EmailAddress = (await _userManager.GetEmailAsync((await _userManager.GetUserAsync(User))!))!;
        model.Basket = _basket.Basket!;

        if (!ModelState.IsValid)
        {
            model.AcceptRefundPolicy = false;
            return View(model);
        }

        var expMonth = (int)model.ExpMonth!;
        var expYear = (int)model.ExpYear!;
        var cardValidity = new DateTime(expYear, expMonth, DateTime.DaysInMonth(expYear, expMonth));
        if (DateTime.Today > cardValidity.Date)
        {
            ModelState.AddModelError(string.Empty, "This card has expired, please choose a different one");
        }

        if (!ModelState.IsValid)
        {
            model.AcceptRefundPolicy = false;
            return View(model);
        }

        await _basket.CloseBasket();
        return RedirectToAction(nameof(Index));
    }
}