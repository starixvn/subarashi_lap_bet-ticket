using Fanex.Athena.BetTicket.Detail.MetaModels;
using Fanex.Athena.BetTicket.Detail.ViewModels;
using Fanex.Athena.BetTicket.Core.Attributes;
using Fanex.Athena.BetTicket.Core.ConsoleClient;
using Fanex.Athena.BetTicket.Core.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Fanex.Athena.BetTicket.Core.Extensions;

namespace Fanex.Athena.BetTicket.Detail.Controllers
{
    [Authorize(Policy = PolicyExtensions.ViewDetail)]
    public class NewCashOutBetListDetailController : Controller
    {
        [HttpGet]
        [AutoBindingMetaModel(bindAgent: true, bindBetFromIP: true)]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ViewResult> Index([FromServices] IBetListApiClient betListApiClient, [FromServices] IContextService contextService,
            NewCashOutBetListDetailMetaModel metaModel)
        {
            metaModel.HasPlayerComm = contextService.IsAllowedCustomerComm;
            metaModel.CurrencyId = contextService.CurrencyList.FirstOrDefault(_ => _.Value == metaModel.Currency).Key;
            var responseData = await betListApiClient.Post<NewCashOutBetListDetailViewModel>("/CashOutBetList/History", metaModel, contextService.Language);
            return View(responseData);
        }
    }
}