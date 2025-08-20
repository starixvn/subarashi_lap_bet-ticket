using Fanex.Athena.BetTicket.Detail.MetaModels;
using Xunit;

namespace Fanex.Athena.BetTicket.Detail.UnitTests.MetaModels
{
    public class CashOutBetListDetailMetaModelTests
    {
        [Fact]
        public void Constructor_ShouldInitializeSuccessfully()
        {
            // Arrange & Act
            var metaModel = new CashOutBetListDetailMetaModel();

            // Assert
            Assert.NotNull(metaModel);
        }

        [Fact]
        public void BaseCurrency_ShouldBeSettableAndGettable()
        {
            // Arrange
            var metaModel = new CashOutBetListDetailMetaModel();
            var expectedBaseCurrency = "USD";

            // Act
            metaModel.BaseCurrency = expectedBaseCurrency;

            // Assert
            Assert.Equal(expectedBaseCurrency, metaModel.BaseCurrency);
        }

        [Fact]
        public void TransId_ShouldBeSettableAndGettable()
        {
            // Arrange
            var metaModel = new CashOutBetListDetailMetaModel();
            var expectedTransId = 123456789L;

            // Act
            metaModel.TransId = expectedTransId;

            // Assert
            Assert.Equal(expectedTransId, metaModel.TransId);
        }

        [Fact]
        public void RefNo_ShouldBeSettableAndGettable()
        {
            // Arrange
            var metaModel = new CashOutBetListDetailMetaModel();
            var expectedRefNo = 987654321L;

            // Act
            metaModel.RefNo = expectedRefNo;

            // Assert
            Assert.Equal(expectedRefNo, metaModel.RefNo);
        }

        [Fact]
        public void BetType_ShouldBeSettableAndGettable()
        {
            // Arrange
            var metaModel = new CashOutBetListDetailMetaModel();
            var expectedBetType = 1;

            // Act
            metaModel.BetType = expectedBetType;

            // Assert
            Assert.Equal(expectedBetType, metaModel.BetType);
        }

        [Fact]
        public void BetdaqId_ShouldBeSettableAndGettable()
        {
            // Arrange
            var metaModel = new CashOutBetListDetailMetaModel();
            var expectedBetdaqId = 555666777L;

            // Act
            metaModel.BetdaqId = expectedBetdaqId;

            // Assert
            Assert.Equal(expectedBetdaqId, metaModel.BetdaqId);
        }

        [Fact]
        public void AgentListXml_ShouldBeSettableAndGettable()
        {
            // Arrange
            var metaModel = new CashOutBetListDetailMetaModel();
            var expectedAgentListXml = "<agents><agent>123</agent></agents>";

            // Act
            metaModel.AgentListXml = expectedAgentListXml;

            // Assert
            Assert.Equal(expectedAgentListXml, metaModel.AgentListXml);
        }

        [Fact]
        public void IsCashOut_ShouldBeSettableAndGettable()
        {
            // Arrange
            var metaModel = new CashOutBetListDetailMetaModel();

            // Act & Assert - Test true
            metaModel.IsCashOut = true;
            Assert.True(metaModel.IsCashOut);

            // Act & Assert - Test false
            metaModel.IsCashOut = false;
            Assert.False(metaModel.IsCashOut);
        }

        [Fact]
        public void HasPlayerCom_ShouldBeSettableAndGettable()
        {
            // Arrange
            var metaModel = new CashOutBetListDetailMetaModel();

            // Act & Assert - Test true
            metaModel.HasPlayerCom = true;
            Assert.True(metaModel.HasPlayerCom);

            // Act & Assert - Test false
            metaModel.HasPlayerCom = false;
            Assert.False(metaModel.HasPlayerCom);
        }

        [Fact]
        public void IsShowIpVN_ShouldBeSettableAndGettable()
        {
            // Arrange
            var metaModel = new CashOutBetListDetailMetaModel();

            // Act & Assert - Test true
            metaModel.IsShowIpVN = true;
            Assert.True(metaModel.IsShowIpVN);

            // Act & Assert - Test false
            metaModel.IsShowIpVN = false;
            Assert.False(metaModel.IsShowIpVN);
        }

        [Fact]
        public void IsShowIp_ShouldBeSettableAndGettable()
        {
            // Arrange
            var metaModel = new CashOutBetListDetailMetaModel();

            // Act & Assert - Test true
            metaModel.IsShowIp = true;
            Assert.True(metaModel.IsShowIp);

            // Act & Assert - Test false
            metaModel.IsShowIp = false;
            Assert.False(metaModel.IsShowIp);
        }

        [Fact]
        public void IsValid_ShouldAlwaysReturnTrue()
        {
            // Arrange
            var metaModel = new CashOutBetListDetailMetaModel();

            // Act
            var isValid = metaModel.IsValid;

            // Assert
            Assert.True(isValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("USD")]
        [InlineData("EUR")]
        [InlineData("GBP")]
        public void BaseCurrency_WithVariousValues_ShouldBeSet(string baseCurrency)
        {
            // Arrange
            var metaModel = new CashOutBetListDetailMetaModel();

            // Act
            metaModel.BaseCurrency = baseCurrency;

            // Assert
            Assert.Equal(baseCurrency, metaModel.BaseCurrency);
        }

        [Theory]
        [InlineData(0L)]
        [InlineData(-1L)]
        [InlineData(long.MaxValue)]
        [InlineData(long.MinValue)]
        public void TransId_WithVariousValues_ShouldBeSet(long transId)
        {
            // Arrange
            var metaModel = new CashOutBetListDetailMetaModel();

            // Act
            metaModel.TransId = transId;

            // Assert
            Assert.Equal(transId, metaModel.TransId);
        }

        [Theory]
        [InlineData(0L)]
        [InlineData(-1L)]
        [InlineData(long.MaxValue)]
        [InlineData(long.MinValue)]
        public void RefNo_WithVariousValues_ShouldBeSet(long refNo)
        {
            // Arrange
            var metaModel = new CashOutBetListDetailMetaModel();

            // Act
            metaModel.RefNo = refNo;

            // Assert
            Assert.Equal(refNo, metaModel.RefNo);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void BetType_WithVariousValues_ShouldBeSet(int betType)
        {
            // Arrange
            var metaModel = new CashOutBetListDetailMetaModel();

            // Act
            metaModel.BetType = betType;

            // Assert
            Assert.Equal(betType, metaModel.BetType);
        }

        [Theory]
        [InlineData(0L)]
        [InlineData(-1L)]
        [InlineData(long.MaxValue)]
        [InlineData(long.MinValue)]
        public void BetdaqId_WithVariousValues_ShouldBeSet(long betdaqId)
        {
            // Arrange
            var metaModel = new CashOutBetListDetailMetaModel();

            // Act
            metaModel.BetdaqId = betdaqId;

            // Assert
            Assert.Equal(betdaqId, metaModel.BetdaqId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("<agents></agents>")]
        [InlineData("<agents><agent>123</agent><agent>456</agent></agents>")]
        public void AgentListXml_WithVariousValues_ShouldBeSet(string agentListXml)
        {
            // Arrange
            var metaModel = new CashOutBetListDetailMetaModel();

            // Act
            metaModel.AgentListXml = agentListXml;

            // Assert
            Assert.Equal(agentListXml, metaModel.AgentListXml);
        }

        [Fact]
        public void AllProperties_WhenSetTogether_ShouldMaintainValues()
        {
            // Arrange
            var metaModel = new CashOutBetListDetailMetaModel();
            var expectedBaseCurrency = "USD";
            var expectedTransId = 123456789L;
            var expectedRefNo = 987654321L;
            var expectedBetType = 1;
            var expectedBetdaqId = 555666777L;
            var expectedAgentListXml = "<agents><agent>123</agent></agents>";

            // Act
            metaModel.BaseCurrency = expectedBaseCurrency;
            metaModel.TransId = expectedTransId;
            metaModel.RefNo = expectedRefNo;
            metaModel.BetType = expectedBetType;
            metaModel.BetdaqId = expectedBetdaqId;
            metaModel.AgentListXml = expectedAgentListXml;
            metaModel.IsCashOut = true;
            metaModel.HasPlayerCom = true;
            metaModel.IsShowIpVN = true;
            metaModel.IsShowIp = true;

            // Assert
            Assert.Equal(expectedBaseCurrency, metaModel.BaseCurrency);
            Assert.Equal(expectedTransId, metaModel.TransId);
            Assert.Equal(expectedRefNo, metaModel.RefNo);
            Assert.Equal(expectedBetType, metaModel.BetType);
            Assert.Equal(expectedBetdaqId, metaModel.BetdaqId);
            Assert.Equal(expectedAgentListXml, metaModel.AgentListXml);
            Assert.True(metaModel.IsCashOut);
            Assert.True(metaModel.HasPlayerCom);
            Assert.True(metaModel.IsShowIpVN);
            Assert.True(metaModel.IsShowIp);
            Assert.True(metaModel.IsValid);
        }
    }
}
