using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Fanex.Athena.BetTicket.Core.ConsoleClient;
using Fanex.Athena.BetTicket.Core.Services;
using Fanex.Athena.BetTicket.Detail.Controllers;
using Fanex.Athena.BetTicket.Detail.MetaModels;
using Fanex.Athena.BetTicket.Detail.ViewModels;
using Fanex.Athena.BetTicket.UnitTests.Common;
using Fanex.Athena.BetTicket.UnitTests.Common.AutoData;
using Moq;
using Xunit;

namespace Fanex.Athena.BetTicket.Detail.UnitTests.Controllers
{
    public class CashOutBetListDetailControllerTest
    {
        [Theory, AutoContextData]
        public async Task Index_WhenCall_ReturnView(
            CashOutBetListDetailViewModel expected,
            [Frozen]Mock<IBetListApiClient> betListApiClient,
            Mock<IContextService> contextService,
            CashOutBetListDetailMetaModel metaModel,
            [Greedy] CashOutBetListDetailController controller)
        {
            // Arrange
            betListApiClient.Setup(client => client.Post<CashOutBetListDetailViewModel>(
                                                                           It.IsAny<string>(),
                                                                           It.IsAny<object>(),
                                                                           It.IsAny<string>())).Returns(Task.FromResult(expected));
            
            // Act
            var result = await controller.Index(betListApiClient.Object, contextService.Object, metaModel);

            // Assert
            Assert.Equal(expected, result.Model);
        }
    }
}
