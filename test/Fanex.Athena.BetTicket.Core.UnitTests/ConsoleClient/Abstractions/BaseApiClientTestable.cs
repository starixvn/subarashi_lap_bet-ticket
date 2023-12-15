using System;
using System.Net.Http;
using System.Threading.Tasks;
using Fanex.Athena.BetTicket.Core.ConsoleClient;

namespace Fanex.Athena.BetTicket.Core.UnitTests.ConsoleClient.Abstractions
{
    public class BaseApiClientTestable : BaseApiClient
    {
        public BaseApiClientTestable(HttpClient httpClient) : base(httpClient) { }

        protected override string ApiName { get; }

        public Task<T> InvokeHandleAsync<T>(string path, object value, string language, MediaType mediaType,
                                            Func<HttpResponseMessage, Task<T>> codeBlock,
                                            bool disposeHttpResponse = true, int timeOut = 100)
        {
            return HandleAsync(path, value, language, mediaType, codeBlock, disposeHttpResponse, timeOut);
        }
    }
}
