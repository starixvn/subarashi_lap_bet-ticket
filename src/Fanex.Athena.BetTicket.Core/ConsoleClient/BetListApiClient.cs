using Fanex.Athena.BetTicket.Core.Services;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Fanex.Athena.BetTicket.Core.ConsoleClient
{
    public class BetListApiClient : BaseApiClient, IBetListApiClient
    {
        private readonly IContextService _contextService;

        public BetListApiClient(HttpClient client, IContextService contextService) : base(client)
        {
            _contextService = contextService;
        }

        protected override string ApiName => "Bet List";

        protected override Task PrepareHeaders(HttpRequestHeaders headers)
        {
            headers.Add("X-SiteDBServer", _contextService.SiteDBServer);
            return Task.CompletedTask;
        }

        public virtual async Task<string> PostString(string path, object value, string language, int timeOut = 100)
        {
            return await HandleAsync(path, value, language, MediaType.Json,
                async (response) =>
                {
                    using (response)
                    {
                        using (var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                        {
                            using (var streamReader = new StreamReader(stream))
                            {
                                using (var textWriter = new StringWriter())
                                {
                                    const int chunkLength = 4096;
                                    var buffer = new char[chunkLength];
                                    int readChunkLength;
                                    do
                                    {
                                        readChunkLength = streamReader.ReadBlock(buffer, 0, chunkLength);
                                        textWriter.Write(buffer, 0, readChunkLength);
                                    } while (readChunkLength > 0);

                                    return textWriter.ToString();
                                }
                            }
                        }
                    }
                },
                timeOut: timeOut
            );
        }

        public async Task<Stream> PostStream(string path, object value, string language, int timeOut = 100)
        {
            return await HandleAsync(path, value, language, MediaType.Json, async (response) =>
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    Stream emptyStream = new MemoryStream();
                    return emptyStream;
                }
                return await response.Content.ReadAsStreamAsync();
            }
          , false, timeOut).ConfigureAwait(false);
        }
    }
}