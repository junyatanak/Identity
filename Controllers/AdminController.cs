using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    public class AdminController : Controller
    {

        private UserManager<AppUser> userManager;
        private IPasswordHasher<AppUser> passwordHasher;
        public AdminController(UserManager<AppUser> usrMgr, IPasswordHasher<AppUser> passwordHash)
        {
            userManager = usrMgr;
            passwordHasher = passwordHash;
        }
        public IActionResult Index()
        {
            return View(userManager.Users);
        }

        public ViewResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            if(ModelState.IsValid)
            {
                AppUser appUser = new AppUser
                {
                    UserName = user.Name,
                    Email = user.Email
                };
                IdentityResult result = await userManager.CreateAsync(appUser, user.Password);
                if(result.Succeeded)
                    return RedirectToAction("Index");
                else
                {
                    foreach(IdentityError error in result.Errors)
                    ModelState.AddModelError("",error.Description);
                }
            }
            return View(user);
            
        }

    }
}
