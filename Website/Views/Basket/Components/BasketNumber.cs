using Microsoft.AspNetCore.Mvc;
using Website.Services;

namespace Website.Views.Basket.Components;

/* Using View Component for the count of items in the basket.
 * This is due to the fact that the number shows up in the _Layout.
 * I have tried different ways but none worked.
 * First I have tried using @Html.Action() as suggested by https://stackoverflow.com/a/10552824
 * Which didn't work because that is only available in Asp.Net (this project is using Asp.Net Core)
 * So I have settled on using View Component as suggested by https://stackoverflow.com/a/68056579
 *
 * I am not 100% sure this is OK with the CA spec., but I have used partials everywhere else.
 */

public class BasketNumber(IBasketService basketService) : ViewComponent
{
    private readonly IBasketService _basketService = basketService;

    public IViewComponentResult Invoke()
    {
        return View(_basketService.ProductCount);
    }
}