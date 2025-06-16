using Fanex.Athena.BetTicket.Core.ConsoleClient;
using Fanex.Athena.BetTicket.Result.MetaModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Fanex.Athena.BetTicket.Result.Controllers
{
    [Authorize]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class SportsResultController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index([FromServices] IBetListApiClient betlistClient, SportsResultMetaModel metaModel)
        {
            const string apiPath = "/ResultReport/ResultPopup";
            var result = await betlistClient.PostStringWithStatus(apiPath, metaModel, metaModel.Language);

            if (result.StatusCode == StatusCodes.Status202Accepted)
            {
                return Json(new { Success = true, RedirectUrl = result.Content });
            }

            ViewBag.Data = result.Content;
            return View();
        }
    }
}