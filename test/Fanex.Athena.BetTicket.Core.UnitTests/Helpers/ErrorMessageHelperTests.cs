using System;
using System.Net;
using Fanex.Athena.BetTicket.Core.Helpers;
using Fanex.Athena.BetTicket.UnitTests.Common;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace Fanex.Athena.BetTicket.Core.UnitTests.Helpers
{
    public class ErrorMessageHelperTests
    {
        [Fact]
        public void GenerateMessage_WithAnyException_BuildMessage()
        {
            // Arrange
            var ctx = new Mock<HttpContext>();
            var request = new Mock<HttpRequest>();
            request.Setup(x => x.Host).Returns(new HostString("Any", 69696));
            request.Setup(x => x.Path).Returns("/Any");
            ctx.Setup(x => x.Request).Returns(request.Object);

            var connection = new Mock<ConnectionInfo>();
            connection.Setup(x => x.RemoteIpAddress).Returns(FakeHelper.Create<IPAddress>());
            ctx.Setup(x => x.Connection).Returns(connection.Object);

            var exception = FakeHelper.Create<MockException>();

            // Act
            ErrorMessageHelper.GenerateMessage(exception, ctx.Object);

            // Assert
            request.VerifyGet(x => x.Host);
            request.VerifyGet(x => x.Path);
            connection.VerifyGet(x => x.RemoteIpAddress);

            Assert.True(exception.IsCallMessage);
            Assert.True(exception.IsCallSource);
            Assert.True(exception.IsCallStackTrace);
        }

        public class MockException : Exception
        {
            public bool IsCallMessage;
            public bool IsCallSource;
            public bool IsCallStackTrace;

            public override string Message {
                get {
                    IsCallMessage = true;

                    return "Any";
                }
            }

            public override string Source {
                get {
                    IsCallSource = true;

                    return "Any";
                }
            }

            public override string StackTrace {
                get {
                    IsCallStackTrace = true;

                    return "Any";
                }
            }
        }
    }
}
