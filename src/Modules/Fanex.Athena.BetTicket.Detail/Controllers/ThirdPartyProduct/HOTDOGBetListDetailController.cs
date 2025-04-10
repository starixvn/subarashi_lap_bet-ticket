namespace Fanex.Athena.BetTicket.Detail.Controllers.ThirdPartyProduct
{
    using Constants;
    using Core.ConsoleClient;
    using Core.Resources;
    using Core.Services;
    using MetaModels;

    public class HOTDOGBetListDetailController : BetListDetailControllerBase
    {
        public HOTDOGBetListDetailController(IBetListApiClient betListClient, IContextService contextService)
            : base(betListClient, contextService)
        {
        }

        public override string StoreProcedure => "Lrf_BetList_RNG_Detail";
        protected override int[] HiddenColumns => new[] { BetListColumn.LicenseeWLColumn };
        protected override string ProductName => ViewResources.ProductHOT;
        protected override string ExcelFileName => "HOTDOGDetail";

        protected override ShowingOption ShowingOption => new()
        {
            CustomerCommVisible = _contextService.IsAllowedCustomerComm
        };
    }
}