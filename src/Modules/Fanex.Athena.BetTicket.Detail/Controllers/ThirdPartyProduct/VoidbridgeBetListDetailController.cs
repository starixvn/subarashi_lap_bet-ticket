using Fanex.Athena.BetTicket.Detail.Constants;
using Fanex.Athena.BetTicket.Detail.MetaModels;
using Fanex.Athena.BetTicket.Infrastructure.ConsoleClient;
using Fanex.Athena.BetTicket.Infrastructure.Resources;
using Fanex.Athena.BetTicket.Infrastructure.Services;

namespace Fanex.Athena.BetTicket.Detail.Controllers
{
    public class VoidbridgeBetListDetailController : BetListDetailControllerBase
    {
        public VoidbridgeBetListDetailController(IBetListApiClient betlistClient, IContextService contextService)
            : base(betlistClient, contextService)
        {
        }

        public override string StoreProcedure => "Lrf_BetList_RNG_Detail";
        protected override int[] HiddenColumns => new[] { BetListColumn.LicenseeWLColumn };
        protected override string ProductName => ViewResources.ProductVB;
        protected override string ExcelFileName => "MacauGamesDetail";

        protected override ShowingOption ShowingOption => new ShowingOption
        {
            CustomerCommVisible = _contextService.IsAllowedCustomerComm
        };
    }
}