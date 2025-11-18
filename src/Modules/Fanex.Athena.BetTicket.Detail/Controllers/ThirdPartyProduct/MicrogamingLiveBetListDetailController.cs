using Fanex.Athena.BetTicket.Detail.Constants;
using Fanex.Athena.BetTicket.Detail.MetaModels;
using Fanex.Athena.BetTicket.Core.ConsoleClient;
using Fanex.Athena.BetTicket.Core.Resources;
using Fanex.Athena.BetTicket.Core.Services;
namespace Fanex.Athena.BetTicket.Detail.Controllers
{
    public class MicrogamingLiveBetListDetailController : BetListDetailControllerBase
    {
        public MicrogamingLiveBetListDetailController(IBetListApiClient betlistClient, IContextService contextService, IResourceService resourceService)
            : base(betlistClient, contextService, resourceService)
        {
        }

        public override string StoreProcedure => "Lrf_BetList_LC_Detail";
        protected override int[] HiddenColumns => new[] { BetListColumn.LicenseeWLColumn };
        protected override string ProductName => ViewResources.ProductMGLC;
        protected override string ExcelFileName => "MicrogamingLiveDetail";

        protected override ShowingOption ShowingOption => new ShowingOption
        {
            CustomerCommVisible = _contextService.IsAllowedCustomerComm
        };
    }
}