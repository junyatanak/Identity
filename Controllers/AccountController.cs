using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        

        // GET: AccountController
        public ActionResult Index()
        {
            return View();
        }

    }
}
