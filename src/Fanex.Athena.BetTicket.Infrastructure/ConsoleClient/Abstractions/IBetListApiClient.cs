using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Fanex.Athena.BetTicket.Infrastructure.ConsoleClient
{
    public interface IBetListApiClient
    {
        Task<T> Post<T>(string path, object value, string language) where T : class;

        Task<string> PostString(string path, object value, string language, int timeOut = 100);

        Task<Stream> PostStream(string path, object value, string language, int timeOut = 100);
    }
}