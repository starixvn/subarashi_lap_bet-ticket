using System.IO;
using System.Threading.Tasks;

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