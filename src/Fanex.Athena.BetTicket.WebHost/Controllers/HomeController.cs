using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.BetTicket.WebHost.Controllers
{
    public class HomeController : ControllerBase
    {
        public HomeController()
        {

        }

        [HttpGet]
        [Authorize]
        public string Index()
        {
            return "tesst";
        }
    }
}
