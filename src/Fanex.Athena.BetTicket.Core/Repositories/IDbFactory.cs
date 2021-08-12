using Fanex.Data;

namespace Fanex.Athena.BetTicket.Core.Repositories
{
    public interface IDbFactory
    {
        IObjectDb GetObjectDb(string spName, int timeout = 120);
    }
}