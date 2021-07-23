using Fanex.Athena.BetTicket.Detail.Constants;
using Fanex.Athena.BetTicket.Detail.MetaModels;
using Fanex.Athena.BetTicket.Core.ConsoleClient;
using Fanex.Athena.BetTicket.Core.Resources;
using Fanex.Athena.BetTicket.Core.Services;

namespace Fanex.Athena.BetTicket.Detail.Controllers
{
    public class SabaClubJackpotWonBetListDetailController : BetListDetailControllerBase
    {
        public SabaClubJackpotWonBetListDetailController(IBetListApiClient betlistClient, IContextService contextService)
            : base(betlistClient, contextService)
        {
        }

        public override string StoreProcedure => "Lrf_BetList_Jackpot_Detail";
        protected override int[] HiddenColumns => new[] { BetListColumn.LicenseeWLColumn };
        protected override string ExcelFileName => "SabaClub_JackpotWonDetails";
        protected override ShowingOption ShowingOption => new ShowingOption
        {
            CustomerCommVisible = _contextService.IsAllowedCustomerComm
        };

        protected override string BuildTitle(string betDate) => $"{ViewResources.ProductSBC} - {ViewResources.JackpotWonDetails}";
    }
}