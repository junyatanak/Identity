using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Identity.Controllers
{
    public class AdminController : Controller
    {

        private UserManager<AppUser> userManager;
        private IPasswordValidator<AppUser> passwordValidator;
        private IUserValidator<AppUser> userValidator;
        public AdminController(UserManager<AppUser> usrMgr, IPasswordValidator<AppUser> passwordVal, IUserValidator<AppUser> userValid)
        {
            userManager = usrMgr;
            passwordValidator = passwordVal;
            userValidator = userValid;
        }
        public IActionResult Index()
        {
            return View(userManager.Users);
        }

        public ViewResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel createUserViewModel)
        {
            if(!ModelState.IsValid)
                return View(createUserViewModel);

            AppUser appUser = new AppUser
            {
                UserName = createUserViewModel.Name,
                Email = createUserViewModel.Email,
                Country = createUserViewModel.Country!.Value,
                Age = createUserViewModel.Age!.Value,
                Salary = createUserViewModel.Salary
            };

            var validPass = await passwordValidator.ValidateAsync(userManager,appUser,createUserViewModel.Password);
            if(!validPass.Succeeded)
            {
                Errors(validPass);
                return View(createUserViewModel);
            }

            IdentityResult result = await userManager.CreateAsync(appUser, createUserViewModel.Password);

            if(result.Succeeded)
                return RedirectToAction("Index");
            else
            {
                foreach(IdentityError error in result.Errors)
                ModelState.AddModelError("",error.Description);
                return View(createUserViewModel);
            }
            
        }

        public async Task<IActionResult> Update(string id)
        {
            AppUser? user = await userManager.FindByIdAsync(id);
            if(user != null)
            {
                var updateUserViewModel = new UpdateUserViewModel
                {
                    Id = user.Id,
                    Email = user.Email ?? string.Empty,
                    Age = user.Age,
                    Country = user.Country,
                    Salary = user.Salary
                };
                return View(updateUserViewModel);
            }
            else
                return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateUserViewModel updateUserViewModel)
        {
            if(!ModelState.IsValid)
                return View(updateUserViewModel);

            var user = await userManager.FindByIdAsync(updateUserViewModel.Id);
            if(user is null)
            {
                ModelState.AddModelError("", "User Not Found");
                return View(updateUserViewModel);
            }
            
            user.Email = updateUserViewModel.Email;
            user.Age = updateUserViewModel.Age!.Value;
            user.Country = updateUserViewModel.Country!.Value;
            user.Salary = updateUserViewModel.Salary;

            if(!string.IsNullOrEmpty(updateUserViewModel.Password))
            {
                var validPass = await passwordValidator.ValidateAsync(userManager,user,updateUserViewModel.Password);
                if(!validPass.Succeeded)
                {
                    Errors(validPass);
                    return View(updateUserViewModel);
                }

                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                var passResult = await userManager.ResetPasswordAsync(user,token,updateUserViewModel.Password);
                if (!passResult.Succeeded)
                {
                    Errors(passResult);
                    return View(updateUserViewModel);
                }
            }

            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                Errors(result);
                return  View(updateUserViewModel);
            }
            
            return RedirectToAction("Index");


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
