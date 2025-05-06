namespace Fanex.Athena.BetTicket.Detail.Controllers
{
    using Core.ConsoleClient;
    using Core.Resources;
    using Core.Services;
    using MetaModels;

    public class BonusBetListDetailController : BetListDetailControllerBase
    {
        public BonusBetListDetailController(IBetListApiClient betlistClient, IContextService contextService)
            : base(betlistClient, contextService)
        {
        }

        public override string StoreProcedure => "Lrf_BetList_LicenseeBonus_Detail";
        protected override int[] HiddenColumns => new int[] { };
        protected override string ExcelFileName => "BonusTransactionDetail";

        protected override ShowingOption ShowingOption => new()
        {
            GrandTotalVisible = false,
            CustomerCommVisible = _contextService.IsAllowedCustomerComm
        };

        protected override string BuildTitle(string betDate, string sportTypeName = default) => ViewResources.TransactionDetail;
    }
}