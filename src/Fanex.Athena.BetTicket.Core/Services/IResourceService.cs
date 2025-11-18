namespace Fanex.Athena.BetTicket.Core.Services
{
    public interface IResourceService
    {
        string GetSportName(object sportId, string languageCode = null);
    }
}