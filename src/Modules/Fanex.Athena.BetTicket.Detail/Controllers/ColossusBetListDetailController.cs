using Fanex.Athena.BetTicket.Detail.MetaModels;
using Fanex.Athena.BetTicket.Detail.ViewModels;
using Fanex.Athena.BetTicket.Infrastructure.Attributes;
using Fanex.Athena.BetTicket.Infrastructure.ConsoleClient;
using Fanex.Athena.BetTicket.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fanex.Athena.BetTicket.Detail.Controllers
{
    public class ColossusBetListDetailController : Controller
    {
        [HttpGet]
        [Authorize(Roles = "super_admin,ColossusBetsWinLoss71")]
        [DataValidation(validateCustId: true)]
        [AutoBindingMetaModel(bindAgent: true)]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ViewResult> Index([FromServices] IBetListApiClient betListApiClient, [FromServices] IContextService contextService,
            ColossusBetListDetailMetaModel metaModel)
        {
            metaModel.UserId = contextService.UserId;
            var responseData = await betListApiClient.Post<ColossusBetListDetailViewModel>("/ColossusBetBetList/Detail", metaModel, contextService.Language);
            return View(responseData);
        }
    }
}