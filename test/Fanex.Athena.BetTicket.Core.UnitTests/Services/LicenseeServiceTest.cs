using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Fanex.Athena.BetTicket.Core.Repositories;
using Fanex.Athena.BetTicket.Core.Services;
using Fanex.Athena.BetTicket.UnitTests.Common;
using Fanex.Athena.BetTicket.UnitTests.Common.AutoData;
using Fanex.Data;
using Moq;
using Xunit;

namespace Fanex.Athena.BetTicket.Core.UnitTests.Services
{
    public class LicenseeServiceTest
    {
        [Theory] [AutoMoqData]
        public async Task CheckCustomerBelongsTo_RootAgent_ReturnTrue(
            [Greedy] LicenseeService service,
            IReadOnlyDictionary<int, int[]> agentList)
        {
            var custId = 0;
            long transId = 0;

            var result = await service.CheckCustomerBelongsTo(agentList, custId, transId);

            Assert.True(result);
        }

        [Theory] [AutoMoqData]
        public async Task CheckCustomerBelongsTo_CustomerBelongs_ReturnTrue(
            Mock<IDataReader> dataReader,
            Mock<IObjectDb> objDb,
            Mock<IDbFactory> dbFactory,
            Dictionary<int, int[]> agentList)
        {
            // Arrange
            var custId = 1;
            long transId = 1;
            var actualCurrencyId = FakeHelper.Create<int>();
            var agentId = FakeHelper.Create<int>();
            agentList[actualCurrencyId] = new[] { agentId };

            dataReader.Setup(r => r.Read()).Returns(true);
            dataReader.Setup(r => r["CurrencyId"]).Returns(actualCurrencyId);
            dataReader.Setup(r => r["CustId"]).Returns(agentId);

            objDb.Setup(x => x.ExecuteReaderAsync(null, null, It.IsAny<CancellationToken>()))
                 .Returns(Task.FromResult(dataReader.Object));
            dbFactory.Setup(x => x.GetObjectDb(It.IsAny<string>(), 120)).Returns(objDb.Object);
            var service = new LicenseeService(dbFactory.Object);

            // Act
            var result = await service.CheckCustomerBelongsTo(agentList, custId, transId);

            // Assert
            Assert.True(result);
            objDb.Verify(x => x.SetParameterValue(nameof(custId).ToLower(), custId));
            objDb.Verify(x => x.SetParameterValue(nameof(transId).ToLower(), transId));
        }

        [Theory]
        [AutoMoqData]
        public async Task CheckCustomerBelongsTo_NoCustomer_ReturnFalse(
            Mock<IDataReader> dataReader,
            Mock<IObjectDb> objDb,
            Mock<IDbFactory> dbFactory,
            Dictionary<int, int[]> agentList)
        {
            // Arrange
            var custId = 1;
            long transId = 1;
            var actualCurrencyId = FakeHelper.Create<int>();
            var agentId = FakeHelper.Create<int>();
            agentList[actualCurrencyId] = new[] { agentId };

            objDb.Setup(x => x.ExecuteReaderAsync(null, null, It.IsAny<CancellationToken>()))
                 .Returns(Task.FromResult(dataReader.Object));
            dbFactory.Setup(x => x.GetObjectDb(It.IsAny<string>(), 120)).Returns(objDb.Object);
            var service = new LicenseeService(dbFactory.Object);

            // Act
            var result = await service.CheckCustomerBelongsTo(agentList, custId, transId);

            // Assert
            Assert.False(result);
        }
    }
}
