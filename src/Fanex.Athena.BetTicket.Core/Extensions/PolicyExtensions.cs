using Microsoft.AspNetCore.Authorization;

namespace Fanex.Athena.BetTicket.Core.Extensions
{
    public static class PolicyExtensions
    {
        public const string ViewDetail = "ViewDetail";

        public static void ViewDetailPolicy(this AuthorizationOptions options)
        {
            options.AddPolicy(ViewDetail, policy => policy.RequireClaim("scope", "betticket_api"));
        }
    }
}
