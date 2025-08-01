using Fanex.Athena.BetTicket.Detail.MetaModels;
using Xunit;

namespace Fanex.Athena.BetTicket.Detail.UnitTests.MetaModels
{
    public class SystemParlayBetListDetailMetaModelTests
    {
        [Fact]
        public void Constructor_ShouldInitializeSuccessfully()
        {
            // Arrange & Act
            var metaModel = new SystemParlayBetListDetailMetaModel();

            // Assert
            Assert.NotNull(metaModel);
        }

        [Fact]
        public void RefNo_ShouldBeSettableAndGettable()
        {
            // Arrange
            var metaModel = new SystemParlayBetListDetailMetaModel();
            var expectedRefNo = 123456789L;

            // Act
            metaModel.RefNo = expectedRefNo;

            // Assert
            Assert.Equal(expectedRefNo, metaModel.RefNo);
        }

        [Fact]
        public void BaseCurrency_ShouldBeSettableAndGettable()
        {
            // Arrange
            var metaModel = new SystemParlayBetListDetailMetaModel();
            var expectedBaseCurrency = "USD";

            // Act
            metaModel.BaseCurrency = expectedBaseCurrency;

            // Assert
            Assert.Equal(expectedBaseCurrency, metaModel.BaseCurrency);
        }

        [Fact]
        public void AgentListXml_ShouldBeSettableAndGettable()
        {
            // Arrange
            var metaModel = new SystemParlayBetListDetailMetaModel();
            var expectedAgentListXml = "<agents><agent>123</agent></agents>";

            // Act
            metaModel.AgentListXml = expectedAgentListXml;

            // Assert
            Assert.Equal(expectedAgentListXml, metaModel.AgentListXml);
        }

        [Fact]
        public void IsValid_WithPositiveRefNo_ShouldReturnTrue()
        {
            // Arrange
            var metaModel = new SystemParlayBetListDetailMetaModel
            {
                RefNo = 123456789L
            };

            // Act
            var isValid = metaModel.IsValid;

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void IsValid_WithZeroRefNo_ShouldReturnFalse()
        {
            // Arrange
            var metaModel = new SystemParlayBetListDetailMetaModel
            {
                RefNo = 0L
            };

            // Act
            var isValid = metaModel.IsValid;

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void IsValid_WithNegativeRefNo_ShouldReturnFalse()
        {
            // Arrange
            var metaModel = new SystemParlayBetListDetailMetaModel
            {
                RefNo = -1L
            };

            // Act
            var isValid = metaModel.IsValid;

            // Assert
            Assert.False(isValid);
        }

        [Theory]
        [InlineData(1L, true)]
        [InlineData(100L, true)]
        [InlineData(long.MaxValue, true)]
        [InlineData(0L, false)]
        [InlineData(-1L, false)]
        [InlineData(-100L, false)]
        [InlineData(long.MinValue, false)]
        public void IsValid_WithVariousRefNoValues_ShouldReturnExpectedResult(long refNo, bool expectedIsValid)
        {
            // Arrange
            var metaModel = new SystemParlayBetListDetailMetaModel
            {
                RefNo = refNo
            };

            // Act
            var isValid = metaModel.IsValid;

            // Assert
            Assert.Equal(expectedIsValid, isValid);
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
            var metaModel = new SystemParlayBetListDetailMetaModel();

            // Act
            metaModel.BaseCurrency = baseCurrency;

            // Assert
            Assert.Equal(baseCurrency, metaModel.BaseCurrency);
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
            var metaModel = new SystemParlayBetListDetailMetaModel();

            // Act
            metaModel.AgentListXml = agentListXml;

            // Assert
            Assert.Equal(agentListXml, metaModel.AgentListXml);
        }

        [Fact]
        public void AllProperties_WhenSetTogether_ShouldMaintainValues()
        {
            // Arrange
            var metaModel = new SystemParlayBetListDetailMetaModel();
            var expectedRefNo = 123456789L;
            var expectedBaseCurrency = "USD";
            var expectedAgentListXml = "<agents><agent>123</agent></agents>";

            // Act
            metaModel.RefNo = expectedRefNo;
            metaModel.BaseCurrency = expectedBaseCurrency;
            metaModel.AgentListXml = expectedAgentListXml;

            // Assert
            Assert.Equal(expectedRefNo, metaModel.RefNo);
            Assert.Equal(expectedBaseCurrency, metaModel.BaseCurrency);
            Assert.Equal(expectedAgentListXml, metaModel.AgentListXml);
            Assert.True(metaModel.IsValid);
        }

        [Fact]
        public void IsValid_ShouldNotBeAffectedByOtherProperties()
        {
            // Arrange
            var metaModel = new SystemParlayBetListDetailMetaModel
            {
                RefNo = 123456789L,
                BaseCurrency = null,
                AgentListXml = null
            };

            // Act
            var isValid = metaModel.IsValid;

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void IsValid_WithValidRefNoAndOtherProperties_ShouldReturnTrue()
        {
            // Arrange
            var metaModel = new SystemParlayBetListDetailMetaModel
            {
                RefNo = 123456789L,
                BaseCurrency = "USD",
                AgentListXml = "<agents><agent>123</agent></agents>"
            };

            // Act
            var isValid = metaModel.IsValid;

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void IsValid_WithInvalidRefNoAndOtherProperties_ShouldReturnFalse()
        {
            // Arrange
            var metaModel = new SystemParlayBetListDetailMetaModel
            {
                RefNo = 0L,
                BaseCurrency = "USD",
                AgentListXml = "<agents><agent>123</agent></agents>"
            };

            // Act
            var isValid = metaModel.IsValid;

            // Assert
            Assert.False(isValid);
        }
    }
}
