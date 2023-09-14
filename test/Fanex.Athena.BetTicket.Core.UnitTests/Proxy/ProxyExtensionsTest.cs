using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Fanex.Athena.BetTicket.UnitTests.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Proxy;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Fanex.Athena.BetTicket.Core.UnitTests.Proxy
{
    public class ProxyExtensionsTest
    {
        [Fact]
        public async Task ProxyRequest_OnNullContext_ThrowArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => ((HttpContext)null).ProxyRequest(It.IsAny<Uri>()));
        }

        [Fact]
        public async Task ProxyRequest_OnNullDestination_ThrowArgumentNullException()
        {
            // Arrange
            HttpContext context = new DefaultHttpContext();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => context.ProxyRequest(null));
        }

        [Fact]
        public async Task ProxyRequest_ValidArgument_SendProxyHttpRequest()
        {
            // Arrange
            var fakeMessageHandler = new FakeHttpMessageHandler();
            var sharedProxyOptions = new SharedProxyOptions
            {
                MessageHandler = fakeMessageHandler
            };

            var options = new Mock<IOptions<SharedProxyOptions>>();
            options.Setup(x => x.Value).Returns(sharedProxyOptions);
            options.Object.Value.PrepareRequest = (_, _) => Task.CompletedTask;
            var proxyService = new ProxyService(options.Object);

            var services = new Mock<IServiceProvider>();
            services.Setup(x => x.GetService(typeof(ProxyService))).Returns(proxyService);

            HttpContext context = new DefaultHttpContext
            {
                RequestServices = services.Object,
                Request = { Method = HttpMethods.Get }
            };
            var destinationUri = FakeHelper.Create<Uri>();

            // Act 
            await context.ProxyRequest(destinationUri);

            // Assert
            Assert.Equal((int)fakeMessageHandler.Response.StatusCode, context.Response.StatusCode);
        }

        public class FakeHttpMessageHandler : HttpMessageHandler
        {
            public readonly HttpResponseMessage Response = new();

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                                   CancellationToken cancellationToken)
            {
                return Task.FromResult(Response);
            }
        }
    }
}
