using Fanex.RemotingRef;
using System.Globalization;

namespace Fanex.Athena.BetTicket.Core.Services
{
    public class ResourceService(IRemotingRefService remotingRefService) : IResourceService
    {
        private readonly IRemotingRefService _remotingRefService = remotingRefService;

        public string GetSportName(object sportId, string languageCode = null)
        {
            if (!IsValidId(sportId))
            {
                return string.Empty;
            }

            return _remotingRefService.GetSportName(sportId.ToString(), GetCurrentIBCLanguageCode(languageCode), string.Empty);
        }

        private static bool IsValidId(object id)
        {
            return id != null && id.ToString() != string.Empty && id.ToString() != "0";
        }

        private static string GetCurrentIBCLanguageCode(string languageCode = null)
        {
            return string.IsNullOrEmpty(languageCode) ? CultureInfo.CurrentUICulture.Name : languageCode;
        }
    }
}