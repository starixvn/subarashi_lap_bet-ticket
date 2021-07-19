using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Athena.BetTicket.WebHost.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        [Authorize]
        public string Index() => "BT is running";

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int code = 500)
        {
            return View(code);
        }
    }
}