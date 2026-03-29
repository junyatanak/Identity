using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Identity.Models;
using Microsoft.AspNetCore.Authorization;

namespace Identity.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
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

    [Authorize]
    public IActionResult Secured()
    {
        return View((object)"Hello");
    }
}
