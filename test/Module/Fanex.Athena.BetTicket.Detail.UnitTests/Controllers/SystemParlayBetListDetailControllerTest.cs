using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Fanex.Athena.BetTicket.Core.ConsoleClient;
using Fanex.Athena.BetTicket.Core.Services;
using Fanex.Athena.BetTicket.Detail.Controllers;
using Fanex.Athena.BetTicket.Detail.MetaModels;
using Fanex.Athena.BetTicket.UnitTests.Common;
using Fanex.Athena.BetTicket.UnitTests.Common.AutoData;
using Moq;
using Xunit;

namespace Fanex.Athena.BetTicket.Detail.UnitTests.Controllers
{
    public class SystemParlayBetListDetailControllerTest
    {
        [Theory, AutoContextData]
        public async Task Index_WhenCall_ReturnView(
            string expected,
            [Frozen] Mock<IBetListApiClient> betListApiClient,
            Mock<IContextService> contextService,
            SystemParlayBetListDetailMetaModel metaModel,
            [Greedy] SystemParlayBetListDetailController controller)
        {
            // Arrange
            betListApiClient.Setup(client => client.PostString(
                                    It.IsAny<string>(),
                                    metaModel,
                                    It.IsAny<string>(),
                                    100)).Returns(Task.FromResult(expected));

            // Act
            await controller.Index(betListApiClient.Object, contextService.Object, metaModel);

            // Assert
            Assert.Equal(expected, controller.ViewBag.DetailText);
        }
    }
}
