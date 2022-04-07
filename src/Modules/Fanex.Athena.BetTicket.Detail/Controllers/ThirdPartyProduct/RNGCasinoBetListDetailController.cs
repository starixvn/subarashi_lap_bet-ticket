using Fanex.Athena.BetTicket.Detail.Constants;
using Fanex.Athena.BetTicket.Detail.MetaModels;
using Fanex.Athena.BetTicket.Core.ConsoleClient;
using Fanex.Athena.BetTicket.Core.Resources;
using Fanex.Athena.BetTicket.Core.Services;

namespace Fanex.Athena.BetTicket.Detail.Controllers
{
    public class RNGCasinoBetListDetailController : BetListDetailControllerBase
    {
        public RNGCasinoBetListDetailController(IBetListApiClient betlistClient, IContextService contextService)
            : base(betlistClient, contextService)
        {
        }

        public override string StoreProcedure => "Lrf_BetList_RNG_Detail";
        protected override int[] HiddenColumns => new[] { BetListColumn.LicenseeWLColumn };
        protected override string ProductName => ViewResources.ProductCS;
        protected override string ExcelFileName => "SabaCasinoDetail";
        protected override ShowingOption ShowingOption => new ShowingOption
        {
            CustomerCommVisible = _contextService.IsAllowedCustomerComm
        };
    }
}