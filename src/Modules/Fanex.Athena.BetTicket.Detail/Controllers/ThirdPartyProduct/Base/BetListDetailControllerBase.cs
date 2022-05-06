using Fanex.Athena.BetTicket.Detail.Constants;
using Fanex.Athena.BetTicket.Detail.MetaModels;
using Fanex.Athena.BetTicket.Detail.ViewModels;
using Fanex.Athena.BetTicket.Core.Attributes;
using Fanex.Athena.BetTicket.Core.ConsoleClient;
using Fanex.Athena.BetTicket.Core.Resources;
using Fanex.Athena.BetTicket.Core.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Fanex.Athena.BetTicket.Detail.Controllers
{
    [Authorize]
    public abstract class BetListDetailControllerBase : Controller
    {
        private readonly IBetListApiClient _betlistClient;
        protected readonly IContextService _contextService;

        protected BetListDetailControllerBase(IBetListApiClient betlistClient, IContextService contextService)
        {
            _betlistClient = betlistClient;
            _contextService = contextService;
        }

        [HttpGet]
        [AutoBindingMetaModel(bindPlayerComm: true)]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public ViewResult Index(BetListDetailMetaModel metaModel)
        {
            AssignDataToViewBag();
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
                    LicenseeWL = responseData.Total.SumLicenseeWl.ToString("#,##0.00")
                },
                TotalRows = responseData.Main.Count()
            }, new JsonSerializerOptions { PropertyNamingPolicy = null });
        }

        [HttpPost]
        [DataValidation(validateCustId: true)]
        [AutoBindingMetaModel(bindAgent: true, bindPlayerComm: true, bindBetFromIP: true, bindLicenseeId: true)]
        public async Task<FileResult> Export([FromBody] BetListDetailExportMetaModel metaModel)
        {
            metaModel.UspKey = StoreProcedure;
            metaModel.ShowingOption = ShowingOption;
            metaModel.HiddenColumns = BetListColumn.DefaultHiddenColumnIndexes.Union(HiddenColumns).ToArray();
            metaModel.Title = BuildTitle(metaModel.Date);

            var streamData = await _betlistClient.PostStream("/BetList/ExportExcel", metaModel, _contextService.Language);
            HttpContext.Response.RegisterForDispose(streamData);

            return File(streamData, "application/octet-stream");
        }

        protected abstract int[] HiddenColumns { get; }

        protected virtual string ProductName { get; }

        protected abstract string ExcelFileName { get; }

        public abstract string StoreProcedure { get; }

        protected abstract ShowingOption ShowingOption { get; }

        protected virtual string BuildTitle(string betDate)
            => $"{ProductName} - {ViewResources.DetailedBetlist} - {betDate}";

        private void AssignDataToViewBag()
        {
            ViewBag.ApiUrl = $"{HttpContext.Request.RouteValues["controller"]}/Data";
            ViewBag.ExportUrl = $"{HttpContext.Request.RouteValues["controller"]}/Export";
            ViewBag.HiddenColumns = HiddenColumns.Select(_ => BetListColumn.ColumnNames[_]);
            ViewBag.IsShowGrandTotal = ShowingOption.GrandTotalVisible;
            ViewBag.ExportFileName = ExcelFileName;
        }
    }
}