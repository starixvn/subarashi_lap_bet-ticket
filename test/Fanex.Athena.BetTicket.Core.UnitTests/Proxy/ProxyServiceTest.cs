using System;
using Microsoft.AspNetCore.Proxy;
using Xunit;

namespace Fanex.Athena.BetTicket.Core.UnitTests.Proxy
{
    public class ProxyServiceTest
    {
        [Fact]
        public void ProxyService_CreateWithNullOption_ThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ProxyService(null));
        }
    }
}
