using Fanex.Athena.BetTicket.Detail.MetaModels;
using Fanex.Athena.BetTicket.Detail.ViewModels;
using Fanex.Athena.BetTicket.Core.Attributes;
using Fanex.Athena.BetTicket.Core.ConsoleClient;
using Fanex.Athena.BetTicket.Core.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Fanex.Athena.BetTicket.Detail.Controllers
{
    [Authorize]
    public class CashOutBetListDetailController : Controller
    {
        [HttpGet]
        [AutoBindingMetaModel(bindAgent: true, bindBetFromIP: true)]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ViewResult> Index([FromServices] IBetListApiClient betListApiClient, [FromServices] IContextService contextService,
            CashOutBetListDetailMetaModel metaModel)
        {
            metaModel.HasPlayerComm = contextService.IsAllowedCustomerComm;
            var responseData = await betListApiClient.Post<CashOutBetListDetailViewModel>("/CashOutBetList/Detail", metaModel, contextService.Language);
            return View(responseData);
        }
    }
}