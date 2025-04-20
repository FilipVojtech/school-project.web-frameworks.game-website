using Microsoft.AspNetCore.Mvc;

namespace Website.Controllers;

public class ReviewsController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}