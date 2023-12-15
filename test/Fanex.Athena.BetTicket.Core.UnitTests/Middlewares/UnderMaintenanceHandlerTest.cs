using System.Threading.Tasks;
using Fanex.Athena.BetTicket.Core.Middlewares;
using Fanex.Athena.BetTicket.UnitTests.Common;
using Fanex.Athena.BetTicket.UnitTests.Common.AutoData;
using Microsoft.AspNetCore.Http;
using Moq;
using ONEbook.UM.Models;
using ONEbook.UM.Services;
using Xunit;

namespace Fanex.Athena.BetTicket.Core.UnitTests.MiddleWares
{
    public class UnderMaintenanceHandlerTest
    {
        [Theory] [AutoContextData]
        public async Task Invoke_NotUM_InvokeNext(
            HttpContext httpContext,
            Mock<RequestDelegate> next,
            Mock<IMaintenanceService> maintenanceService)
        {
            // Arrange
            maintenanceService.Setup(x => x.IsUnderMaintenanceAsync()).Returns(Task.FromResult(false));
            maintenanceService.Setup(x => x.GetUnderMaintenanceTimeAsync())
                              .Returns(Task.FromResult(FakeHelper.Create<UnderMaintenanceTime>()));

            var handler = new UnderMaintenanceHandler(next.Object, maintenanceService.Object);

            // Act
            await handler.Invoke(httpContext);

            // Assert
            next.Verify(x => x.Invoke(httpContext));
        }

        [Theory] [AutoContextData]
        public async Task Invoke_WithUM_FormatWeb(
            HttpContext httpContext,
            Mock<RequestDelegate> next,
            Mock<IMaintenanceService> maintenanceService)
        {
            // Arrange
            maintenanceService.Setup(x => x.IsUnderMaintenanceAsync()).Returns(Task.FromResult(true));
            maintenanceService.Setup(x => x.GetUnderMaintenanceTimeAsync())
                              .Returns(Task.FromResult(FakeHelper.Create<UnderMaintenanceTime>()));
            var handler = new UnderMaintenanceHandler(next.Object, maintenanceService.Object);

            // Act
            await handler.Invoke(httpContext);

            // Assert
            Assert.Equal("text/html", httpContext.Response.ContentType);
        }

        [Theory] [AutoContextData]
        public async Task Invoke_WithAjaxUM_FormatWeb(
            HttpContext httpContext,
            Mock<RequestDelegate> next,
            Mock<IMaintenanceService> maintenanceService)
        {
            // Arrange
            httpContext.Request.Headers["X-Requested-With"] = "XMLHttpRequest";
            maintenanceService.Setup(x => x.IsUnderMaintenanceAsync()).Returns(Task.FromResult(true));
            maintenanceService.Setup(x => x.GetUnderMaintenanceTimeAsync())
                              .Returns(Task.FromResult(FakeHelper.Create<UnderMaintenanceTime>()));
            var underMaintenanceHandler = new UnderMaintenanceHandler(next.Object, maintenanceService.Object);

            // Act
            await underMaintenanceHandler.Invoke(httpContext);

            // Assert
            Assert.Equal("application/json", httpContext.Response.ContentType);
        }
    }
}
