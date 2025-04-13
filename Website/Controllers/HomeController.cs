using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Website.Data;
using Website.Models;
using Website.Models.ViewModels;

namespace Website.Controllers;

public class HomeController(ILogger<HomeController> logger, ApplicationDbContext context) : Controller
{
    private readonly ILogger<HomeController> _logger = logger;

    public IActionResult Index()
    {
        var reviews = context.Reviews
            .AsNoTracking()
            .Include(r => r.Game)
            .Include(r => r.Author)
            .Take(25)
            .ToList();

        var homeVM = new HomeViewModel
        {
            Reviews = reviews,
        };

        return View(homeVM);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}