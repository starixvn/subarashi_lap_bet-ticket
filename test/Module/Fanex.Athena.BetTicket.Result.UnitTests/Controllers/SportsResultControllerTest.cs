using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Fanex.Athena.BetTicket.Core.ConsoleClient;
using Fanex.Athena.BetTicket.Result.Controllers;
using Fanex.Athena.BetTicket.Result.MetaModels;
using Fanex.Athena.BetTicket.UnitTests.Common.AutoData;
using Moq;
using Xunit;

namespace Fanex.Athena.BetTicket.Result.UnitTests.Controllers
{
    public class SportsResultControllerTest
    {
        [Theory, AutoContextData]
        public async Task Index_WhenCall_ReturnView(
            string expected,
            [Frozen] Mock<IBetListApiClient> betListApiClient,
            SportsResultMetaModel metaModel,
            [Greedy] SportsResultController controller)
        {
            // Arrange
            betListApiClient.Setup(client => client.PostString(
                                    It.IsAny<string>(),
                                    metaModel,
                                    It.IsAny<string>(),
                                    100)).Returns(Task.FromResult(expected));
            // Act
            await controller.Index(betListApiClient.Object, metaModel);

            // Assert
            Assert.Equal(expected, controller.ViewBag.Data);
        }
    }
}
