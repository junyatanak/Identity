using System.Security.Claims;
using Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    [Authorize]
    public class ClaimsController : Controller
    {
        private UserManager<AppUser> userManager;
        public ClaimsController(UserManager<AppUser> userMgr)
        {
            userManager = userMgr;
        }
        public IActionResult Index() => View(User?.Claims);

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(string claimType, string claimValue)
        {
            var user = await userManager.GetUserAsync(User);
            var claim = new Claim(claimType,claimValue,ClaimValueTypes.String);
        
        }

    }
}
