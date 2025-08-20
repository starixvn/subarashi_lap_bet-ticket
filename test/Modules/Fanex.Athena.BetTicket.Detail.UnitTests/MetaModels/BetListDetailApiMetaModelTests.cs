using System;
using Fanex.Athena.BetTicket.Detail.MetaModels;
using Xunit;

namespace Fanex.Athena.BetTicket.Detail.UnitTests.MetaModels
{
    public class BetListDetailApiMetaModelTests
    {
        [Fact]
        public void Constructor_ShouldInitializeWithDefaultValues()
        {
            // Arrange & Act
            var metaModel = new BetListDetailApiMetaModel();

            // Assert
            Assert.Equal(1, metaModel.PageIndex);
            Assert.Equal(int.MaxValue - 1, metaModel.PageSize);
        }

        [Fact]
        public void PageIndex_ShouldBeSettableAndGettable()
        {
            // Arrange
            var metaModel = new BetListDetailApiMetaModel();
            var expectedPageIndex = 5;

            // Act
            metaModel.PageIndex = expectedPageIndex;

            // Assert
            Assert.Equal(expectedPageIndex, metaModel.PageIndex);
        }

        [Fact]
        public void PageSize_ShouldBeSettableAndGettable()
        {
            // Arrange
            var metaModel = new BetListDetailApiMetaModel();
            var expectedPageSize = 100;

            // Act
            metaModel.PageSize = expectedPageSize;

            // Assert
            Assert.Equal(expectedPageSize, metaModel.PageSize);
        }

        [Fact]
        public void UspKey_ShouldBeSettableAndGettable()
        {
            // Arrange
            var metaModel = new BetListDetailApiMetaModel();
            var expectedUspKey = "Lrf_BetList_LC_Detail";

            // Act
            metaModel.UspKey = expectedUspKey;

            // Assert
            Assert.Equal(expectedUspKey, metaModel.UspKey);
        }

        [Fact]
        public void Date_ShouldBeSettableAndGettable()
        {
            // Arrange
            var metaModel = new BetListDetailApiMetaModel();
            var expectedDate = DateTime.Parse("2024-01-01");

            // Act
            metaModel.Date = expectedDate;

            // Assert
            Assert.Equal(expectedDate, metaModel.Date);
        }

        [Fact]
        public void SportTypeId_ShouldBeSettableAndGettable()
        {
            // Arrange
            var metaModel = new BetListDetailApiMetaModel();
            var expectedSportTypeId = 1;

            // Act
            metaModel.SportTypeId = expectedSportTypeId;

            // Assert
            Assert.Equal(expectedSportTypeId, metaModel.SportTypeId);
        }

        [Fact]
        public void SportTypeName_ShouldBeSettableAndGettable()
        {
            // Arrange
            var metaModel = new BetListDetailApiMetaModel();
            var expectedSportTypeName = "Football";

            // Act
            metaModel.SportTypeName = expectedSportTypeName;

            // Assert
            Assert.Equal(expectedSportTypeName, metaModel.SportTypeName);
        }

        [Fact]
        public void BaseCurrency_ShouldBeSettableAndGettable()
        {
            // Arrange
            var metaModel = new BetListDetailApiMetaModel();
            var expectedBaseCurrency = "USD";

            // Act
            metaModel.BaseCurrency = expectedBaseCurrency;

            // Assert
            Assert.Equal(expectedBaseCurrency, metaModel.BaseCurrency);
        }

        [Fact]
        public void CustId_ShouldBeSettableAndGettable()
        {
            // Arrange
            var metaModel = new BetListDetailApiMetaModel();
            var expectedCustId = 12345;

            // Act
            metaModel.CustId = expectedCustId;

            // Assert
            Assert.Equal(expectedCustId, metaModel.CustId);
        }

        [Fact]
        public void TransId_ShouldBeSettableAndGettable()
        {
            // Arrange
            var metaModel = new BetListDetailApiMetaModel();
            var expectedTransId = 987654321L;

            // Act
            metaModel.TransId = expectedTransId;

            // Assert
            Assert.Equal(expectedTransId, metaModel.TransId);
        }

        [Fact]
        public void TransType_ShouldBeSettableAndGettable()
        {
            // Arrange
            var metaModel = new BetListDetailApiMetaModel();
            var expectedTransType = 1L;

            // Act
            metaModel.TransType = expectedTransType;

            // Assert
            Assert.Equal(expectedTransType, metaModel.TransType);
        }

        [Fact]
        public void RefCode_ShouldBeSettableAndGettable()
        {
            // Arrange
            var metaModel = new BetListDetailApiMetaModel();
            var expectedRefCode = 123456789L;

            // Act
            metaModel.RefCode = expectedRefCode;

            // Assert
            Assert.Equal(expectedRefCode, metaModel.RefCode);
        }

        [Fact]
        public void AgentListXml_ShouldBeSettableAndGettable()
        {
            // Arrange
            var metaModel = new BetListDetailApiMetaModel();
            var expectedAgentListXml = "<agents><agent>123</agent></agents>";

            // Act
            metaModel.AgentListXml = expectedAgentListXml;

            // Assert
            Assert.Equal(expectedAgentListXml, metaModel.AgentListXml);
        }

        [Fact]
        public void HasPlayerCom_ShouldBeSettableAndGettable()
        {
            // Arrange
            var metaModel = new BetListDetailApiMetaModel();

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
            var metaModel = new BetListDetailApiMetaModel();

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
            var metaModel = new BetListDetailApiMetaModel();

            // Act & Assert - Test true
            metaModel.IsShowIp = true;
            Assert.True(metaModel.IsShowIp);

            // Act & Assert - Test false
            metaModel.IsShowIp = false;
            Assert.False(metaModel.IsShowIp);
        }



        [Fact]
        public void IsValid_ShouldReturnTrue()
        {
            // Arrange
            var metaModel = new BetListDetailApiMetaModel();

            // Act
            var isValid = metaModel.IsValid;

            // Assert
            Assert.True(isValid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void PageIndex_WithInvalidValues_ShouldStillBeSet(int invalidPageIndex)
        {
            // Arrange
            var metaModel = new BetListDetailApiMetaModel();

            // Act
            metaModel.PageIndex = invalidPageIndex;

            // Assert
            Assert.Equal(invalidPageIndex, metaModel.PageIndex);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void PageSize_WithInvalidValues_ShouldStillBeSet(int invalidPageSize)
        {
            // Arrange
            var metaModel = new BetListDetailApiMetaModel();

            // Act
            metaModel.PageSize = invalidPageSize;

            // Assert
            Assert.Equal(invalidPageSize, metaModel.PageSize);
        }

        [Fact]
        public void UspKey_WithNullValue_ShouldBeSet()
        {
            // Arrange
            var metaModel = new BetListDetailApiMetaModel();

            // Act
            metaModel.UspKey = null;

            // Assert
            Assert.Null(metaModel.UspKey);
        }

        [Fact]
        public void UspKey_WithEmptyString_ShouldBeSet()
        {
            // Arrange
            var metaModel = new BetListDetailApiMetaModel();

            // Act
            metaModel.UspKey = string.Empty;

            // Assert
            Assert.Equal(string.Empty, metaModel.UspKey);
        }
    }
}
