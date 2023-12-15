using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fanex.Athena.BetTicket.UnitTests.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Proxy;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Moq;
using Xunit;

namespace Fanex.Athena.BetTicket.Core.UnitTests.Proxy
{
    public class ProxyAdvancedExtensionsTest
    {
        [Fact]
        public void ToWebSocketScheme_Null_Throw()
        {
            Assert.Throws<ArgumentNullException>(() => ((Uri)null).ToWebSocketScheme());
        }

        [Fact]
        public void ToWebSocketScheme_Http_ReturnWsScheme()
        {
            // Arrange
            const string scheme = "http";

            var uriBuilder = new UriBuilder();
            uriBuilder.Scheme = scheme;
            var uri = uriBuilder.Uri;

            // Act
            var result = uri.ToWebSocketScheme();

            // Assert
            Assert.Equal("ws", result.Scheme);
        }

        [Fact]
        public void ToWebSocketScheme_Https_ReturnWssScheme()
        {
            // Arrange
            const string scheme = "https";

            var uriBuilder = new UriBuilder();
            uriBuilder.Scheme = scheme;
            var uri = uriBuilder.Uri;

            // Act
            var result = uri.ToWebSocketScheme();

            // Assert
            Assert.Equal("wss", result.Scheme);
        }

        [Fact]
        public void AcceptProxyWebSocketRequest_NullContext_ThrowArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() =>
                                                          ((HttpContext)null)
                                                         .AcceptProxyWebSocketRequest(It.IsAny<Uri>()));
        }

        [Fact]
        public void AcceptProxyWebSocketRequest_NullUri_ThrowArgumentNullException()
        {
            var context = new DefaultHttpContext();

            Assert.ThrowsAsync<ArgumentNullException>(() => context.AcceptProxyWebSocketRequest(null));
        }

        [Fact]
        public async Task AcceptProxyWebSocketRequest_NotWebSocketRequest_ThrowInvalidOperationException()
        {
            // Arrange
            var websocket = new Mock<WebSocketManager>();
            websocket.Setup(x => x.IsWebSocketRequest).Returns(false);
            var ctx = new Mock<HttpContext>();
            ctx.Setup(x => x.WebSockets).Returns(websocket.Object);

            var destinationUri = FakeHelper.Create<Uri>();

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                                                                    ctx.Object
                                                                       .AcceptProxyWebSocketRequest(destinationUri));
        }

        [Fact]
        public async Task AcceptProxyWebSocketRequest_ConnectFailed_ReturnFalse()
        {
            // Arrange
            var ctx = new Mock<HttpContext>();
            ctx.Setup(x => x.WebSockets).Returns(MockWebSocket().Object);
            ctx.Setup(x => x.Request).Returns(MockHttpRequest().Object);
            ctx.Setup(x => x.Response).Returns(new Mock<HttpResponse>().Object);
            ctx.Setup(x => x.RequestServices).Returns(MockRequestService().Object);
            ctx.Setup(x => x.RequestAborted).Returns(new CancellationToken(false));

            // Act
            var destinationUri = FakeHelper.Create<Uri>().ToWebSocketScheme();
            var result = await ctx.Object.AcceptProxyWebSocketRequest(destinationUri);

            // Assert
            Assert.False(result);
        }

        #region TestHelpers

        private static Mock<WebSocketManager> MockWebSocket()
        {
            var websocket = new Mock<WebSocketManager>();
            websocket.Setup(x => x.IsWebSocketRequest).Returns(true);
            websocket.Setup(x => x.WebSocketRequestedProtocols).Returns(new List<string> { "Any" });
            return websocket;
        }

        private static Mock<HttpRequest> MockHttpRequest()
        {
            var httpRequest = new Mock<HttpRequest>();
            httpRequest.Setup(x => x.Headers).Returns(new HeaderDictionary(new Dictionary<string, StringValues>
            {
                { "Any", StringValues.Empty }
            }));
            return httpRequest;
        }

        private static Mock<IServiceProvider> MockRequestService()
        {
            var sharedProxyOptions = new SharedProxyOptions
            {
                WebSocketKeepAliveInterval = TimeSpan.MaxValue
            };

            var options = new Mock<IOptions<SharedProxyOptions>>();
            options.Setup(x => x.Value).Returns(sharedProxyOptions);
            options.Object.Value.PrepareRequest = (_, _) => Task.CompletedTask;
            var proxyService = new ProxyService(options.Object);

            var services = new Mock<IServiceProvider>();
            services.Setup(x => x.GetService(typeof(ProxyService))).Returns(proxyService);
            return services;
        }

        #endregion
    }
}
