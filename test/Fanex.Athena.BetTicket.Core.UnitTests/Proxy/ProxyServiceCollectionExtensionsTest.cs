using System;
using Microsoft.AspNetCore.Proxy;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Fanex.Athena.BetTicket.Core.UnitTests.Proxy
{
    public class ProxyServiceCollectionExtensionsTest
    {
        [Fact]
        public void AddProxy_ValidArguments_AddsProxyServiceAsSingleton()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddProxy(_ => { });

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var proxyService1 = serviceProvider.GetService<ProxyService>();
            var proxyService2 = serviceProvider.GetService<ProxyService>();

            Assert.NotNull(proxyService1);
            Assert.NotNull(proxyService2);
            Assert.Same(proxyService1, proxyService2);
        }

        [Fact]
        public void AddProxy_WhenServicesIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            IServiceCollection services = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => services.AddProxy(_ => { }));
        }

        [Fact]
        public void AddProxy_WhenConfigureOptionsIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => services.AddProxy(null));
        }
    }
}
