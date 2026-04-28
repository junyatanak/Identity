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
            var claimValuesArray = claimValues.Split(";");
            var claimType = claimValuesArray[0];
            var claimValue = claimValuesArray[1];
            var claimIssuer = claimValuesArray[2];

            var claim = User.Claims.Where(x => x.Type == claimType && x.Value == claimValue && x.Issuer == claimIssuer).FirstOrDefault();
            var result = await userManager.RemoveClaimAsync(user,claim);
           
            if (!result.Succeeded)
            {
                Errors(result);
                return View("Index");
            }
                
            return RedirectToAction("Index");

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
