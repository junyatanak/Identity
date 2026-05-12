using System.Security.Claims;
using Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SQLitePCL;

namespace Identity.Controllers
{
    [Authorize]
    public class ClaimsController : Controller
    {
        private UserManager<AppUser> userManager;
        private IAuthorizationService authService;
        public ClaimsController(UserManager<AppUser> userMgr, IAuthorizationService auth)
        {
            userManager = userMgr;
            authService = auth;
        }
        public IActionResult Index() => View(User?.Claims);

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(string claimType, string claimValue)
        {
            var user = await userManager.GetUserAsync(User);
            if(user is null)
            {
                return Unauthorized();
            }

            var claim = new Claim(claimType,claimValue,ClaimValueTypes.String);
            var result = await userManager.AddClaimAsync(user,claim);

            if (!result.Succeeded)
            {
                Errors(result);
                return View();
            }
                
            return RedirectToAction("Index");
        
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string claimValues)
        {
            var user = await userManager.GetUserAsync(User);
            if(user is null)
            {
                return Unauthorized();
            }

            var claimValuesArray = claimValues.Split(";");
            var claimType = claimValuesArray[0];
            var claimValue = claimValuesArray[1];
            var claimIssuer = claimValuesArray[2];

            var claim = User.Claims.Where(x => x.Type == claimType && x.Value == claimValue && x.Issuer == claimIssuer).FirstOrDefault();
            if(claim is null)
            {
                return NotFound();
            }
            var result = await userManager.RemoveClaimAsync(user,claim);
           
            if (!result.Succeeded)
            {
                Errors(result);
                return View("Index");
            }
                
            return RedirectToAction("Index");

        }

        [Authorize(Policy = "AspManager")]
        public IActionResult Project() => View("Index",User?.Claims);

        [Authorize(Policy = "AllowAdmin")]
        public IActionResult AdminFiles() => View("Index",User?.Claims);

        public async Task<IActionResult> PrivateAccess(string title)
        {
            string[] allowedUsers = ["admin", "luffy"];
            var authResult = await authService.AuthorizeAsync(User,allowedUsers,"PrivateAccess");

            if(authResult.Succeeded)
                return View("Index", User.Claims);
            else
                return new ChallengeResult();

        }



        private void Errors(IdentityResult result)
        {
            foreach(IdentityError error in result.Errors)
            {
                ModelState.AddModelError("",error.Description);
            }
        }


    }
}
