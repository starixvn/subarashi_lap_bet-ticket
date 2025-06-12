using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;

namespace Fanex.Athena.BetTicket.Core.ConsoleClient
{
    public interface IBetListApiClient
    {
        Task<T> Post<T>(string path, object value, string language) where T : class;

        Task<string> PostString(string path, object value, string language, int timeOut = 100);

        Task<Stream> PostStream(string path, object value, string language, int timeOut = 100);

        Task<ApiResponse<string>> PostStringWithStatus(string path, object value, string language, int timeOut = 100);
    }
}