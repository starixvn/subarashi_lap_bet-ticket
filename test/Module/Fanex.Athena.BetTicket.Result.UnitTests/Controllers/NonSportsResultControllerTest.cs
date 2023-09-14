using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Fanex.Athena.BetTicket.Core.ConsoleClient;
using Fanex.Athena.BetTicket.Result.Controllers;
using Fanex.Athena.BetTicket.UnitTests.Common.AutoData;
using Moq;
using Xunit;

namespace Fanex.Athena.BetTicket.Result.UnitTests.Controllers
{
    public class NonSportsResultControllerTest
    {
        [Theory, AutoContextData]
        public async Task SAGaming_WhenCall_ReturnView(
            string expected,
            string transDesc,
            string matchCode,
            string betType,
            string language,
            [Frozen]Mock<IBetListApiClient> betlistClient,
            [Greedy]NonSportsResultController controller)
        {
            // Arrange
            var fakeInput = "{rootUrl}" + expected;
            betlistClient.Setup(client => client.PostString(
                                                            It.IsAny<string>(),
                                                            It.IsAny<object>(),
                                                            It.IsAny<string>(),
                                                            100)).Returns(Task.FromResult(fakeInput));

            // Act
            await controller.SAGaming(transDesc, matchCode, betType, language);

            // Assert
            Assert.Equal(expected, controller.ViewBag.Data);
        }

        [Theory, AutoContextData]
        public async Task AllBet_WhenCall_ReturnView(
            string expected,
            string transDesc, 
            string betCheck,
            string betType, 
            string language,
            [Frozen] Mock<IBetListApiClient> betlistClient,
            [Greedy] NonSportsResultController controller)
        {
            // Arrange
            var fakeInput = "{rootUrl}" + expected;
            betlistClient.Setup(client => client.PostString(
                                                            It.IsAny<string>(),
                                                            It.IsAny<object>(),
                                                            It.IsAny<string>(),
                                                            100)).Returns(Task.FromResult(fakeInput));
            // Act
            await controller.AllBet(transDesc, betCheck, betType, language);

            // Assert
            Assert.Equal(expected, controller.ViewBag.Data);
        }

        [Theory, AutoContextData]
        public async Task SexyGaming_WhenCall_ReturnView(
            string expected,
            string transDesc,
            string matchCode, 
            string betType, 
            string language,
            [Frozen] Mock<IBetListApiClient> betlistClient,
            [Greedy] NonSportsResultController controller)
        {
            // Arrange
            var fakeInput = "{rootUrl}" + expected;
            betlistClient.Setup(client => client.PostString(
                                                            It.IsAny<string>(),
                                                            It.IsAny<object>(),
                                                            It.IsAny<string>(),
                                                            100)).Returns(Task.FromResult(fakeInput));

            // Act
            await controller.SexyGaming(transDesc, matchCode, betType, language);

            // Assert
            Assert.Equal(expected, controller.ViewBag.Data);
        }

        [Theory, AutoContextData]
        public async Task WM_WhenCall_ReturnView(
            string expected,
            string transDesc,
            string gameId3rd,
            int betType, 
            string language,
            [Frozen] Mock<IBetListApiClient> betListClient,
            [Greedy] NonSportsResultController controller)
        {
            // Arrange
            var fakeInput = "{rootUrl}" + expected;
            betListClient.Setup(client => client.PostString(
                                                            It.IsAny<string>(),
                                                            It.IsAny<object>(),
                                                            It.IsAny<string>(),
                                                            100)).Returns(Task.FromResult(fakeInput));
            // Act
            await controller.WM(transDesc, gameId3rd, betType, language);

            // Assert
            Assert.Equal(expected, controller.ViewBag.Data);
        }
    }
}
