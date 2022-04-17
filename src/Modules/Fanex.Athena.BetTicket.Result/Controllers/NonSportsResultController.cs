using Fanex.Athena.BetTicket.Core.ConsoleClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fanex.Athena.BetTicket.Result.Controllers
{
    [Authorize]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class NonSportsResultController : Controller
    {
        private readonly IBetListApiClient _betlistClient;

        public NonSportsResultController(IBetListApiClient betlistClient)
        {
            _betlistClient = betlistClient;
        }

        [HttpGet]
        public async Task<ViewResult> SAGaming(string transDesc, string matchCode, string betType, string language)
        {
            var model = new
            {
                transDesc,
                matchCode,
                betType,
                languageCode = language
            };
            const string apiPath = "/SAGamingResultPopup/Index";
            await BindHtmlResult(apiPath, model, language);
            return View();
        }

        [HttpGet]
        public async Task<ViewResult> AllBet(string transDesc, string betCheck, string betType, string language)
        {
            var model = new
            {
                transDesc,
                betCheck,
                betType,
                languageCode = language
            };
            const string apiPath = "/AllBetResultPopup/Index";
            await BindHtmlResult(apiPath, model, language);
            return View();
        }

        [HttpGet]
        public async Task<ViewResult> SexyGaming(string transDesc, string matchCode, string betType, string language)
        {
            var model = new
            {
                transDesc,
                matchCode,
                betType,
                languageCode = language
            };
            const string apiPath = "/SexyGamingResultPopup/Index";
            await BindHtmlResult(apiPath, model, language);
            return View();
        }

        [HttpGet]
        public async Task<ViewResult> WM(string transDesc, string gameId3rd, int betType, string language)
        {
            var model = new
            {
                transDesc,
                gameId3rd,
                betType,
                languageCode = language
            };
            const string apiPath = "/WMResultPopup/Index";
            await BindHtmlResult(apiPath, model, language);
            return View();
        }

        private async Task BindHtmlResult(string apiPath, object model, string language)
        {
            var data = await _betlistClient.PostString(apiPath, model, language);
            ViewBag.Data = data.Replace("{rootUrl}", string.Empty);
        }
    }
}