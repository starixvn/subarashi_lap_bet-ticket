using System;
using System.Threading.Tasks;
using Fanex.Athena.BetTicket.UnitTests.Common;
using Fanex.Athena.BetTicket.UnitTests.Common.AutoData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Proxy;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Fanex.Athena.BetTicket.Core.UnitTests.Proxy
{
    public class ProxyMiddleWareTest
    {
        [Fact]
        public void ProxyMiddleWare_NullOptions_ThrowArgumentNullException()
        {
            // Assert
            var options = new Mock<IOptions<SharedProxyOptions>>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ProxyMiddleware(null, options.Object));
        }

        [Fact]
        public void ProxyMiddleWare_NullRequestDelegate_ThrowArgumentNullException()
        {
            // Assert
            Task Request(HttpContext _)
            {
                return Task.CompletedTask;
            }

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ProxyMiddleware(Request, null));
        }

        [Theory, AutoMoqData]
        public async Task Invoke_NullContext_ThrowArgumentNullException(ProxyMiddleware proxy)
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => proxy.Invoke(null));
        }

        [Theory, AutoContextData]
        public void ProxyMiddleWare_ShouldProxyRequest_InvokeNext(
            HttpContext context,
            Mock<RequestDelegate> next,
            Mock<IOptions<SharedProxyOptions>> options)
        {
            // Assert
            var sharedProxyOptions = new SharedProxyOptions
            {
                ShouldProxyRequest = _ => false
            };

            options.Setup(x => x.Value).Returns(sharedProxyOptions);
            next.Setup(x => x.Invoke(context)).Returns(Task.CompletedTask);
            var proxy = new ProxyMiddleware(next.Object, options.Object);

            // Act
            proxy.Invoke(context);

            // Assert
            next.Verify(x => x.Invoke(context));
        }

        [Theory, AutoContextData]
        public void ProxyMiddleWare_ValidateRequest_OnFailedValidation(
            HttpContext context,
            Mock<RequestDelegate> next,
            Mock<Func<HttpContext, Task>> onFailedValidation,
            Mock<IOptions<SharedProxyOptions>> options)
        {
            // Assert
            onFailedValidation.Setup(x => x.Invoke(context)).Returns(Task.CompletedTask);
            var sharedProxyOptions = new SharedProxyOptions
            {
                ShouldProxyRequest = _ => true,
                ValidateRequest = _ => false,
                OnFailedValidation = onFailedValidation.Object
            };

            options.Setup(x => x.Value).Returns(sharedProxyOptions);
            var proxy = new ProxyMiddleware(next.Object, options.Object);

            // Act
            proxy.Invoke(context);

            // Assert
            onFailedValidation.Verify(x => x.Invoke(context));
        }
    }
}
