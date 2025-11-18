namespace Fanex.Athena.BetTicket.Detail.Controllers
{
    using Constants;
    using Core.ConsoleClient;
    using Core.Resources;
    using Core.Services;
    using MetaModels;

    public class GPILiveCasinoBetListDetailController : BetListDetailControllerBase
    {
        public GPILiveCasinoBetListDetailController(IBetListApiClient betlistClient, IContextService contextService, IResourceService resourceService)
            : base(betlistClient, contextService, resourceService)
        {
        }

        public override string StoreProcedure => "Lrf_BetList_LC_Detail";
        protected override int[] HiddenColumns => new[] { BetListColumn.LicenseeWLColumn };
        protected override string ProductName => ViewResources.ProductGPILC;
        protected override string ExcelFileName => "GPILiveCasinoDetail";

        protected override ShowingOption ShowingOption => new()
        {
            CustomerCommVisible = _contextService.IsAllowedCustomerComm
        };
    }
}