using System;
using Microsoft.AspNetCore.Proxy;
using Xunit;
using static System.Int32;

namespace Fanex.Athena.BetTicket.Core.UnitTests.Proxy
{
    public class ShareProxyOptionsTest
    {
        [Fact]
        public void WebSocketBufferSize_WhenCall_ReturnValue()
        {
            // Arrange
            const int bufferSize = MaxValue;
            var options = new SharedProxyOptions
            {
                WebSocketBufferSize = bufferSize
            };

            // Act & Assert
            Assert.Equal(bufferSize, options.WebSocketBufferSize);
        }

        [Fact]
        public void WebSocketBufferSize_SetInvalidValue_ThrowArgumentOutOfRangeException()
        {
            // Arrange
            const int bufferSize = 0;
            var options = new SharedProxyOptions();

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => options.WebSocketBufferSize = bufferSize);
        }

        [Fact]
        public void WebSocketBufferSize_SetEmptyValue_ReturnValue()
        {
            // Arrange
            int? bufferSize = null;
            var options = new SharedProxyOptions
            {
                WebSocketBufferSize = null
            };

            // Act & Assert
            Assert.Equal(bufferSize, options.WebSocketBufferSize);
        }
    }
}
