using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LMSCapital.Models;
using LMSCapital.Services;

namespace LMSCapital.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly BookService _bookSvc;

    public HomeController(ILogger<HomeController> logger, LMSDbContext context)
    {
        _logger = logger;
        _bookSvc = new BookService(context);
    }

    public IActionResult Index()
    {
        var issuedBooksCount = _bookSvc.GetIssuedBooksCount();
        ViewData["Count"] = issuedBooksCount;
        return View();
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
