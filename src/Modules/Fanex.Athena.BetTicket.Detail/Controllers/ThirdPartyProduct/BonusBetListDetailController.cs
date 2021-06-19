using Fanex.Athena.BetTicket.Detail.MetaModels;
using Fanex.Athena.BetTicket.Infrastructure.ConsoleClient;
using Fanex.Athena.BetTicket.Infrastructure.Resources;
using Fanex.Athena.BetTicket.Infrastructure.Services;

namespace Fanex.Athena.BetTicket.Detail.Controllers
{
    public class BonusBetListDetailController : BetListDetailControllerBase
    {
        public BonusBetListDetailController(IBetListApiClient betlistClient, IContextService contextService)
            : base(betlistClient, contextService)
        {
        }

        public override string StoreProcedure => "Lrf_BetList_LicenseeBonus_Detail";
        protected override int[] HiddenColumns => new int[] { };
        protected override string ExcelFileName => "BonusTransactionDetail";
        protected override ShowingOption ShowingOption => new ShowingOption
        {
            GrandTotalVisible = false,
            CustomerCommVisible = _contextService.IsAllowedCustomerComm
        };

        protected override string BuildTitle(string _) => ViewResources.TransactionDetail;
    }
}