using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Fanex.Athena.BetTicket.UnitTests.Common;
using Fanex.Athena.BetTicket.UnitTests.Common.MockClasses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Proxy;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Moq;
using Xunit;

namespace Fanex.Athena.BetTicket.Core.UnitTests.Proxy
{
    public class ProxyExtensionsTest
    {
        [Fact]
        public void UseProxy_OnNullProxy_ThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => ((IApplicationBuilder)null).UseProxy());
        }

        [Fact]
        public void CreateProxyHttpRequest_StreamContent_ReturnStreamContent()
        {
            var context = new DefaultHttpContext
            {
                Request =
                {
                    Method = HttpMethods.Post,
                    Headers =
                    {
                        FakeHelper.Create<KeyValuePair<string, StringValues>>(),
                        FakeHelper.Create<KeyValuePair<string, StringValues>>()
                    },
                    Body = Stream.Null
                }
            };

            // Act
            var result = context.CreateProxyHttpRequest(FakeHelper.Create<Uri>());

            // Assert
            Assert.IsType<StreamContent>(result.Content);
        }

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
            var httpResponseMessage = FakeHttpResponseMessage();
            var fakeMessageHandler = new MockHttpMessageHandler
            {
                HttpResponseMessage = httpResponseMessage
            };

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
            Assert.Equal((int)httpResponseMessage.StatusCode, context.Response.StatusCode);
        }

        private HttpResponseMessage FakeHttpResponseMessage()
        {
            var response = new HttpResponseMessage();
            response.Headers.Add(FakeHelper.Create<string>(), FakeHelper.CreateMany<string>());
            response.Content.Headers.Add(FakeHelper.Create<string>(), FakeHelper.CreateMany<string>());

            return response;
        }
    }
}
