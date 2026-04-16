using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Identity.Controllers;

public class HomeController : Controller
{
    private UserManager<AppUser> userManager;
    public HomeController(UserManager<AppUser> userMgr)
    {
        userManager = userMgr;
    }
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

    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> Secured()
    {
        AppUser? user = await userManager.GetUserAsync(HttpContext.User);
        string message = "Hello " + user!.UserName;
        return View((object)message);
    }
}
