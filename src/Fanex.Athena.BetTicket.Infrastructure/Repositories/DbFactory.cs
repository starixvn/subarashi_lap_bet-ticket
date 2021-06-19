using Fanex.Data;

namespace Fanex.Athena.BetTicket.Infrastructure.Repositories
{
    public interface IDbFactory
    {
        IObjectDb GetObjectDb(string spName, int timeout = 120);
    }

    public class DbFactory : IDbFactory
    {
        public IObjectDb GetObjectDb(string spName, int timeout = 120) 
            => ObjectDbFactory.CreateInstance(spName, timeout);
    }
}