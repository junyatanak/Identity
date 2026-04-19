using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Identity.Controllers
{
    public class AdminController : Controller
    {

        private UserManager<AppUser> userManager;
        private IPasswordHasher<AppUser> passwordHasher;
        private IPasswordValidator<AppUser> passwordValidator;
        private IUserValidator<AppUser> userValidator;
        public AdminController(UserManager<AppUser> usrMgr, IPasswordHasher<AppUser> passwordHash, IPasswordValidator<AppUser> passwordVal, IUserValidator<AppUser> userValid)
        {
            userManager = usrMgr;
            passwordHasher = passwordHash;
            passwordValidator = passwordVal;
            userValidator = userValid;
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
                    Email = user.Email,
                    Country = user.Country,
                    Age = user.Age,
                    Salary = user.Salary
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

        public async Task<IActionResult> Update(string id)
        {
            AppUser? user = await userManager.FindByIdAsync(id);
            if(user != null)
                return View(user);
            else
                return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Update(string id, string email, string password,int age, Country country,string salary)
        {
            AppUser? user = await userManager.FindByIdAsync(id);
            if(user == null)
            {
                ModelState.AddModelError("", "User Not Found");
                return View(user);
            }
            
            user.Email = email;
            user.Age = age;
            user.Country = country;
            user.Salary = salary;

            IdentityResult validEmail;
            if(!string.IsNullOrEmpty(email))
            {
                validEmail = await userValidator.ValidateAsync(userManager,user);
                if(!validEmail.Succeeded)
                    Errors(validEmail);
            }
            else
                ModelState.AddModelError("","Email cannot be empty.");

            IdentityResult validPass;
            if(!string.IsNullOrEmpty(password))
            {
                validPass = await passwordValidator.ValidateAsync(userManager,user,password);
                if(validPass.Succeeded)
                    user.PasswordHash = passwordHasher.HashPassword(user,password);
                else
                    Errors(validPass);
            }
            else
                ModelState.AddModelError("","Password cannot be empty.");

            var result = await userManager.UpdateAsync(user);
                              
            if(result.Succeeded)
                return RedirectToAction("Index");

            Errors(result);
            return View(user);


        }


        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            AppUser? user = await userManager.FindByIdAsync(id);
            if(user != null)
            {
                IdentityResult result = await userManager.DeleteAsync(user);
                if(result.Succeeded)
                    return RedirectToAction("Index");
                else
                    Errors(result);
            }
            else
                ModelState.AddModelError("","User Not Found");
            return View("Index", userManager.Users);

        }

        private void Errors(IdentityResult result)
        {
            foreach(IdentityError error in result.Errors)
                ModelState.AddModelError("",error.Description);
        }

    }
}
