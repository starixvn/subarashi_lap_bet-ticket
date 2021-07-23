using Fanex.Athena.BetTicket.Detail.MetaModels;
using Fanex.Athena.BetTicket.Core.Attributes;
using Fanex.Athena.BetTicket.Core.ConsoleClient;
using Fanex.Athena.BetTicket.Core.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fanex.Athena.BetTicket.Detail.Controllers
{
    public class SystemParlayBetListDetailController : Controller
    {
        [HttpGet]
        [DataValidation]
        [AutoBindingMetaModel(bindAgent: true)]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ViewResult> Index([FromServices] IBetListApiClient betListApiClient, [FromServices] IContextService contextService,
            SystemParlayBetListDetailMetaModel metaModel)
        {
            var responseData = await betListApiClient.PostString("/SystemParlayBetList/Detail", metaModel, contextService.Language);
            ViewBag.DetailText = responseData;
            return View();
        }
    }
}