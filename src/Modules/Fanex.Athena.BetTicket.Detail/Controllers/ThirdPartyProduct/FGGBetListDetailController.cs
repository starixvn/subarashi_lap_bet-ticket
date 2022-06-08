using Fanex.Athena.BetTicket.Detail.Constants;
using Fanex.Athena.BetTicket.Detail.MetaModels;
using Fanex.Athena.BetTicket.Core.ConsoleClient;
using Fanex.Athena.BetTicket.Core.Resources;
using Fanex.Athena.BetTicket.Core.Services;

namespace Fanex.Athena.BetTicket.Detail.Controllers
{
    public class FGGBetListDetailController : BetListDetailControllerBase
    {
        public FGGBetListDetailController(IBetListApiClient betlistClient, IContextService contextService)
            : base(betlistClient, contextService)
        {
        }

        public override string StoreProcedure => "Lrf_BetList_LC_Detail";
        protected override int[] HiddenColumns => new[] { BetListColumn.LicenseeWLColumn };
        protected override string ProductName => ViewResources.ProductFGGL;
        protected override string ExcelFileName => "FGGDetail";

        protected override ShowingOption ShowingOption => new ShowingOption
        {
            CustomerCommVisible = _contextService.IsAllowedCustomerComm
        };
    }
}