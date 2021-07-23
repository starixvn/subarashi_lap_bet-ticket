using Fanex.Athena.BetTicket.Core.ConsoleClient;
using Fanex.Athena.BetTicket.Result.MetaModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fanex.Athena.BetTicket.Result.Controllers
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class SportsResultController : Controller
    {
        [HttpGet]
        public async Task<ViewResult> Index([FromServices] IBetListApiClient betlistClient, SportsResultMetaModel metaModel)
        {
            const string apiPath = "/ResultReport/ResultPopup";
            var result = await betlistClient.PostString(apiPath, metaModel, metaModel.Language);
            ViewBag.Data = result;
            return View();
        }
    }
}