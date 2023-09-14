using System;
using System.Net;
using System.Threading.Tasks;
using Fanex.Athena.BetTicket.Core.Middlewares;
using Fanex.Athena.BetTicket.UnitTests.Common.AutoData;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Fanex.Athena.BetTicket.Core.UnitTests.MiddleWares
{
    public class ErrorHandlerTest
    {
        [Theory] [AutoContextData]
        public async Task Invoke_WithoutError_InvokeNext(
            HttpContext httpContext,
            Mock<RequestDelegate> next,
            Mock<ILogger<ErrorHandler>> logger)
        {
            //Arrange
            var handler = new ErrorHandler(next.Object, logger.Object);

            //Act
            await handler.Invoke(httpContext);

            //Assert
            next.Verify(x => x.Invoke(httpContext));
        }

        [Theory] [AutoContextData]
        public async Task Invoke_HandleErrorWithException_RedirectResult(
            HttpContext httpContext,
            Exception exception,
            Mock<RequestDelegate> next,
            Mock<ILogger<ErrorHandler>> logger)
        {
            // Arrange
            next.Setup(x => x.Invoke(httpContext)).Throws(exception);
            var handler = new ErrorHandler(next.Object, logger.Object);

            // Act
            await handler.Invoke(httpContext);

            // Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, httpContext.Response.StatusCode);
        }
    }
}
