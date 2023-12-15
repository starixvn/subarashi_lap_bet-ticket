using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Fanex.Athena.BetTicket.UnitTests.Common.MockClasses
{
    public class MockHttpMessageHandler : HttpMessageHandler
    {
        public HttpResponseMessage HttpResponseMessage { get; set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                               CancellationToken cancellationToken)
        {
            return Task.FromResult(HttpResponseMessage);
        }
    }
}
