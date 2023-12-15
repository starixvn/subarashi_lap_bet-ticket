using System.IO;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Fanex.Athena.BetTicket.Core.ConsoleClient;
using Fanex.Athena.BetTicket.Detail.Controllers;
using Fanex.Athena.BetTicket.Detail.MetaModels;
using Fanex.Athena.BetTicket.Detail.ViewModels;
using Fanex.Athena.BetTicket.UnitTests.Common.AutoData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Fanex.Athena.BetTicket.Detail.UnitTests.Controllers.ThirdPartyProduct.Base
{
    public abstract class BetListDetailControllerBaseTest<T> where T : BetListDetailControllerBase
    {
        [Theory] [AutoContextData]
        public void Index_WhenCall_ReturnViewWithViewBag(
            BetListDetailMetaModel metaModel,
            HttpContext context,
            [Greedy] T controller)
        {
            // Arrange
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = context
            };

            // Act
            controller.Index(metaModel);

            // Assert
            Assert.Equal($"{context.Request.RouteValues["controller"]}/Data", controller.ViewBag.ApiUrl);
            Assert.Equal($"{context.Request.RouteValues["controller"]}/Export", controller.ViewBag.ExportUrl);
            Assert.NotNull(controller.ViewBag.HiddenColumns);
            Assert.NotNull(controller.ViewBag.IsShowGrandTotal);
            Assert.NotNull(controller.ViewBag.ExportFileName);
        }

        [Theory] [AutoContextData]
        public async Task Data_WhenCall_ReturnData(
            BetListDetailApiMetaModel metaModel,
            BetListViewModel expected,
            [Frozen] Mock<IBetListApiClient> betListClient,
            [Greedy] T controller)
        {
            // Arrange
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            betListClient.Setup(client => client.Post<BetListViewModel>(
                                                                        It.IsAny<string>(),
                                                                        It.IsAny<object>(),
                                                                        It.IsAny<string>()))
                         .Returns(Task.FromResult(expected));
            // Act
            var result = await controller.Data(metaModel);

            // Assert
            Assert.NotNull(result);
            betListClient.Verify(client => client.Post<BetListViewModel>(
                                                                         It.IsAny<string>(),
                                                                         It.IsAny<object>(),
                                                                         It.IsAny<string>()));
        }

        [Theory] [AutoContextData]
        public async Task Export_WhenCall_ReturnFile(
            BetListDetailExportMetaModel metaModel,
            Stream stream,
            [Frozen] Mock<IBetListApiClient> betListClient,
            [NoAutoProperties] T controller)
        {
            // Arrange
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            betListClient.Setup(client => client.PostStream(
                                                            It.IsAny<string>(),
                                                            It.IsAny<object>(),
                                                            It.IsAny<string>(),
                                                            100)).Returns(Task.FromResult(stream));

            // Act
            var result = await controller.Export(metaModel);

            // Assert
            Assert.Equal("application/octet-stream", result.ContentType);
        }
    }
}
