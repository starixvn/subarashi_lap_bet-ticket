using Fanex.Athena.BetTicket.Detail.MetaModels;
using System;
using Xunit;

namespace Fanex.Athena.BetTicket.Detail.UnitTests.MetaModels
{
    public class BetListDetailMetaModelTests
    {
        [Fact]
        public void Constructor_ShouldInitializeSuccessfully()
        {
            // Arrange & Act
            var metaModel = new BetListDetailMetaModel();

            // Assert
            Assert.NotNull(metaModel);
        }

        [Fact]
        public void SportTypeId_ShouldBeSettableAndGettable()
        {
            // Arrange
            var metaModel = new BetListDetailMetaModel();
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
            var metaModel = new BetListDetailMetaModel();
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
            var metaModel = new BetListDetailMetaModel();
            var expectedBaseCurrency = "USD";

            // Act
            metaModel.BaseCurrency = expectedBaseCurrency;

            // Assert
            Assert.Equal(expectedBaseCurrency, metaModel.BaseCurrency);
        }

        [Fact]
        public void Date_ShouldBeSettableAndGettable()
        {
            // Arrange
            var metaModel = new BetListDetailMetaModel();
            var expectedDate = DateTime.Parse("2024-01-01");

            // Act
            metaModel.Date = expectedDate;

            // Assert
            Assert.Equal(expectedDate, metaModel.Date);
        }

        [Fact]
        public void CustId_ShouldBeSettableAndGettable()
        {
            // Arrange
            var metaModel = new BetListDetailMetaModel();
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
            var metaModel = new BetListDetailMetaModel();
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
            var metaModel = new BetListDetailMetaModel();
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
            var metaModel = new BetListDetailMetaModel();
            var expectedRefCode = 123456789L;

            // Act
            metaModel.RefCode = expectedRefCode;

            // Assert
            Assert.Equal(expectedRefCode, metaModel.RefCode);
        }

        [Fact]
        public void HasPlayerCom_ShouldBeSettableAndGettable()
        {
            // Arrange
            var metaModel = new BetListDetailMetaModel();

            // Act & Assert - Test true
            metaModel.HasPlayerCom = true;
            Assert.True(metaModel.HasPlayerCom);

            // Act & Assert - Test false
            metaModel.HasPlayerCom = false;
            Assert.False(metaModel.HasPlayerCom);
        }

        [Fact]
        public void IsValid_ShouldAlwaysReturnTrue()
        {
            // Arrange
            var metaModel = new BetListDetailMetaModel();

            // Act
            var isValid = metaModel.IsValid;

            // Assert
            Assert.True(isValid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void SportTypeId_WithVariousValues_ShouldBeSet(int sportTypeId)
        {
            // Arrange
            var metaModel = new BetListDetailMetaModel();

            // Act
            metaModel.SportTypeId = sportTypeId;

            // Assert
            Assert.Equal(sportTypeId, metaModel.SportTypeId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("Basketball")]
        [InlineData("Tennis")]
        public void SportTypeName_WithVariousValues_ShouldBeSet(string sportTypeName)
        {
            // Arrange
            var metaModel = new BetListDetailMetaModel();

            // Act
            metaModel.SportTypeName = sportTypeName;

            // Assert
            Assert.Equal(sportTypeName, metaModel.SportTypeName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("USD")]
        [InlineData("EUR")]
        [InlineData("GBP")]
        public void BaseCurrency_WithVariousValues_ShouldBeSet(string baseCurrency)
        {
            // Arrange
            var metaModel = new BetListDetailMetaModel();

            // Act
            metaModel.BaseCurrency = baseCurrency;

            // Assert
            Assert.Equal(baseCurrency, metaModel.BaseCurrency);
        }

        [Fact]
        public void Date_WithMinValue_ShouldBeSet()
        {
            // Arrange
            var metaModel = new BetListDetailMetaModel();
            var minDate = DateTime.MinValue;

            // Act
            metaModel.Date = minDate;

            // Assert
            Assert.Equal(minDate, metaModel.Date);
        }

        [Fact]
        public void Date_WithMaxValue_ShouldBeSet()
        {
            // Arrange
            var metaModel = new BetListDetailMetaModel();
            var maxDate = DateTime.MaxValue;

            // Act
            metaModel.Date = maxDate;

            // Assert
            Assert.Equal(maxDate, metaModel.Date);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void CustId_WithVariousValues_ShouldBeSet(int custId)
        {
            // Arrange
            var metaModel = new BetListDetailMetaModel();

            // Act
            metaModel.CustId = custId;

            // Assert
            Assert.Equal(custId, metaModel.CustId);
        }

        [Theory]
        [InlineData(0L)]
        [InlineData(-1L)]
        [InlineData(long.MaxValue)]
        [InlineData(long.MinValue)]
        public void TransId_WithVariousValues_ShouldBeSet(long transId)
        {
            // Arrange
            var metaModel = new BetListDetailMetaModel();

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
        public void TransType_WithVariousValues_ShouldBeSet(long transType)
        {
            // Arrange
            var metaModel = new BetListDetailMetaModel();

            // Act
            metaModel.TransType = transType;

            // Assert
            Assert.Equal(transType, metaModel.TransType);
        }

        [Theory]
        [InlineData(0L)]
        [InlineData(-1L)]
        [InlineData(long.MaxValue)]
        [InlineData(long.MinValue)]
        public void RefCode_WithVariousValues_ShouldBeSet(long refCode)
        {
            // Arrange
            var metaModel = new BetListDetailMetaModel();

            // Act
            metaModel.RefCode = refCode;

            // Assert
            Assert.Equal(refCode, metaModel.RefCode);
        }
    }
}
