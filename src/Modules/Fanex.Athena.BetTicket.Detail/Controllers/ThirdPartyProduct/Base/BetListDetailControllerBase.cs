namespace Fanex.Athena.BetTicket.Detail.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Constants;
    using Core.Attributes;
    using Core.ConsoleClient;
    using Core.Extensions;
    using Core.Resources;
    using Core.Services;
    using MetaModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using ViewModels;

    [Authorize(Policy = PolicyExtensions.ViewDetail)]
    public abstract class BetListDetailControllerBase : Controller
    {
        private readonly IBetListApiClient _betlistClient;
        protected readonly IContextService _contextService;

        protected BetListDetailControllerBase(IBetListApiClient betlistClient, IContextService contextService)
        {
            _betlistClient = betlistClient;
            _contextService = contextService;
        }

        protected abstract int[] HiddenColumns { get; }

        protected virtual string ProductName { get; }

        protected virtual string ExcelFileName { get; }

        public abstract string StoreProcedure { get; }

        protected abstract ShowingOption ShowingOption { get; }

        [HttpGet]
        [AutoBindingMetaModel(bindPlayerComm: true)]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public ViewResult Index(BetListDetailMetaModel metaModel)
        {
            AssignDataToViewBag(metaModel);
            return View("~/Views/ThirdPartyProductBetListDetail/Index.cshtml", metaModel);
        }

        [HttpPost]
        [DataValidation(validateCustId: true)]
        [AutoBindingMetaModel(bindAgent: true, bindPlayerComm: true, bindBetFromIP: true)]
        public async Task<JsonResult> Data([FromBody] BetListDetailApiMetaModel metaModel)
        {
            metaModel.UspKey = StoreProcedure;
            var responseData = await _betlistClient.Post<BetListViewModel>("/BetList/GetBetList", metaModel, _contextService.Language);
            return new JsonResult(new
            {
                Data = responseData.Main,
                Total = new
                {
                    WinLoss = responseData.Total.SumWinLoss.ToString("#,##0.00"),
                    LicenseeWL = responseData.Total.SumLicenseeWl.ToString("#,##0.00"),
                    Stake = responseData.Total.SumStake,
                    Comm = responseData.Total.SumComm.ToString("#,##0.00")
                },
                TotalRows = responseData.Main.Count()
            });
        }

        [HttpPost]
        [DataValidation(validateCustId: true)]
        [AutoBindingMetaModel(bindAgent: true, bindPlayerComm: true, bindBetFromIP: true, bindLicenseeId: true)]
        public async Task<FileResult> Export([FromBody] BetListDetailExportMetaModel metaModel)
        {
            metaModel.UspKey = StoreProcedure;
            metaModel.ShowingOption = ShowingOption;
            metaModel.HiddenColumns = BetListColumn.DefaultHiddenColumnIndexes.Union(HiddenColumns).ToArray();
            metaModel.Title = BuildTitle(metaModel.Date, metaModel.SportTypeName);

            var streamData = await _betlistClient.PostStream("/BetList/ExportExcel", metaModel, _contextService.Language);
            HttpContext.Response.RegisterForDispose(streamData);

            return File(streamData, "application/octet-stream");
        }

        protected virtual string BuildTitle(string betDate, string sportTypeName = default)
            => $"{(string.IsNullOrEmpty(sportTypeName) ? ProductName : sportTypeName)} - {ViewResources.DetailedBetlist} - {betDate}";

        private void AssignDataToViewBag(BetListDetailMetaModel metaModel)
        {
            ViewBag.ApiUrl = $"{HttpContext.Request.RouteValues["controller"]}/Data";
            ViewBag.ExportUrl = $"{HttpContext.Request.RouteValues["controller"]}/Export";
            ViewBag.HiddenColumns = HiddenColumns.Select(_ => BetListColumn.ColumnNames[_]);
            ViewBag.IsShowGrandTotal = ShowingOption.GrandTotalVisible;
            ViewBag.ExportFileName = string.IsNullOrEmpty(ExcelFileName) ? GetExcelNameBySportTypeName(metaModel.SportTypeName) : ExcelFileName;
        }

        private static string GetExcelNameBySportTypeName(string sportTypeName)
        {
            string fileName = $"{sportTypeName}Detail";
            fileName = fileName.Replace(" ", ""); // remove space in file name
            return fileName;
        }
    }
}