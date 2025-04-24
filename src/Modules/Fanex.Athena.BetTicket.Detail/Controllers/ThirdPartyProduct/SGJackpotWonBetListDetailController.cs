namespace Fanex.Athena.BetTicket.Detail.Controllers
{
    using Constants;
    using Core.ConsoleClient;
    using Core.Resources;
    using Core.Services;
    using MetaModels;

    public class SGJackpotWonBetListDetailController : BetListDetailControllerBase
    {
        public SGJackpotWonBetListDetailController(IBetListApiClient betlistClient, IContextService contextService)
            : base(betlistClient, contextService)
        {
        }

        public override string StoreProcedure => "Lrf_BetList_Jackpot_Detail";
        protected override int[] HiddenColumns => new[] { BetListColumn.LicenseeWLColumn };
        protected override string ExcelFileName => "SG_JackpotWonDetails";

        protected override ShowingOption ShowingOption => new()
        {
            CustomerCommVisible = _contextService.IsAllowedCustomerComm
        };

        protected override string BuildTitle(string betDate, string sportTypeName = default) =>
            $"{ViewResources.ProductVGA} - {ViewResources.JackpotWonDetails}";
    }
}