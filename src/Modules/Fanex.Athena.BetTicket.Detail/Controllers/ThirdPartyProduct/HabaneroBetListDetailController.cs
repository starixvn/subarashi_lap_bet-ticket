using Fanex.Athena.BetTicket.Detail.Constants;
using Fanex.Athena.BetTicket.Detail.MetaModels;
using Fanex.Athena.BetTicket.Core.ConsoleClient;
using Fanex.Athena.BetTicket.Core.Resources;
using Fanex.Athena.BetTicket.Core.Services;
namespace Fanex.Athena.BetTicket.Detail.Controllers
{
    public class HabaneroBetListDetailController : BetListDetailControllerBase
    {
        public HabaneroBetListDetailController(IBetListApiClient betlistClient, IContextService contextService, IResourceService resourceService)
            : base(betlistClient, contextService, resourceService)
        {
        }

        public override string StoreProcedure => "Lrf_BetList_RNG_Detail";
        protected override int[] HiddenColumns => new[] { BetListColumn.LicenseeWLColumn };
        protected override string ProductName => ViewResources.ProductHB;
        protected override string ExcelFileName => "HabaneroDetail";

        protected override ShowingOption ShowingOption => new ShowingOption
        {
            GrandTotalVisible = false,
            CustomerCommVisible = _contextService.IsAllowedCustomerComm
        };
    }
}