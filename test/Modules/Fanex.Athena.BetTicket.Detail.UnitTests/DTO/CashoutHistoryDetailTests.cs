using Fanex.Athena.BetTicket.Detail.DTO;
using Xunit;

namespace Fanex.Athena.BetTicket.Detail.UnitTests.DTO
{
    public class CashoutHistoryDetailTests
    {
        [Fact]
        public void Constructor_WithValidParameters_ShouldInitializeProperties()
        {
            // Arrange
            var changeTime = new DateTime(2024, 1, 15, 14, 30, 45);
            var amount = 1000.50m;
            var actionId = "1";

            // Act
            var cashoutHistory = new CashoutHistoryDetail(changeTime, amount, actionId);

            // Assert
            Assert.Equal(changeTime, cashoutHistory.ChangeTime);
            Assert.Equal(amount, cashoutHistory.CashoutAmount);
            Assert.Equal(actionId, cashoutHistory.ActionId);
        }

        [Fact]
        public void ChangeTimeText_ShouldReturnFormattedDateTime()
        {
            // Arrange
            var changeTime = new DateTime(2024, 1, 15, 14, 30, 45);
            var cashoutHistory = new CashoutHistoryDetail(changeTime, 1000.50m, "1");

            // Act
            var result = cashoutHistory.ChangeTimeText;

            // Assert
            Assert.Equal("1/15/2024 02:30:45 PM", result);
        }

        [Fact]
        public void CashoutAmountText_ShouldReturnFormattedAmount()
        {
            // Arrange
            var cashoutHistory = new CashoutHistoryDetail(DateTime.Now, 1234.56m, "1");

            // Act
            var result = cashoutHistory.CashoutAmountText;

            // Assert
            Assert.Equal("1,234.56", result);
        }

        [Fact]
        public void CashoutAmountText_WithZeroAmount_ShouldReturnFormattedZero()
        {
            // Arrange
            var cashoutHistory = new CashoutHistoryDetail(DateTime.Now, 0m, "1");

            // Act
            var result = cashoutHistory.CashoutAmountText;

            // Assert
            Assert.Equal("0.00", result);
        }

        [Fact]
        public void CashoutAmountText_WithLargeAmount_ShouldReturnFormattedAmount()
        {
            // Arrange
            var cashoutHistory = new CashoutHistoryDetail(DateTime.Now, 1234567.89m, "1");

            // Act
            var result = cashoutHistory.CashoutAmountText;

            // Assert
            Assert.Equal("1,234,567.89", result);
        }

        [Fact]
        public void ActionName_WithActionId1_ShouldReturnCashOut()
        {
            // Arrange
            var cashoutHistory = new CashoutHistoryDetail(DateTime.Now, 1000m, "1");

            // Act
            var result = cashoutHistory.ActionName;

            // Assert
            Assert.Equal("Cash Out", result);
        }

        [Fact]
        public void ActionName_WithActionId6_ShouldReturnCashOutCancelled()
        {
            // Arrange
            var cashoutHistory = new CashoutHistoryDetail(DateTime.Now, 1000m, "6");

            // Act
            var result = cashoutHistory.ActionName;

            // Assert
            Assert.Equal("Cash Out Cancelled ", result);
        }

        [Fact]
        public void ActionName_WithActionId8_ShouldReturnCashOutRecovered()
        {
            // Arrange
            var cashoutHistory = new CashoutHistoryDetail(DateTime.Now, 1000m, "8");

            // Act
            var result = cashoutHistory.ActionName;

            // Assert
            Assert.Equal("Cash Out Recovered ", result);
        }

        [Fact]
        public void ActionName_WithUnknownActionId_ShouldReturnActionId()
        {
            // Arrange
            var unknownActionId = "99";
            var cashoutHistory = new CashoutHistoryDetail(DateTime.Now, 1000m, unknownActionId);

            // Act
            var result = cashoutHistory.ActionName;

            // Assert
            Assert.Equal(unknownActionId, result);
        }

        [Fact]
        public void ActionName_WithEmptyActionId_ShouldReturnEmptyString()
        {
            // Arrange
            var cashoutHistory = new CashoutHistoryDetail(DateTime.Now, 1000m, "");

            // Act
            var result = cashoutHistory.ActionName;

            // Assert
            Assert.Equal("", result);
        }

        [Fact]
        public void ActionName_WithNullActionId_ShouldThrowArgumentNullException()
        {
            // Arrange
            var cashoutHistory = new CashoutHistoryDetail(DateTime.Now, 1000m, null);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => cashoutHistory.ActionName);
        }

        [Fact]
        public void Properties_ShouldBeSettable()
        {
            // Arrange
            var cashoutHistory = new CashoutHistoryDetail(DateTime.Now, 1000m, "1");
            var newChangeTime = new DateTime(2024, 2, 20, 10, 15, 30);
            var newAmount = 2000.75m;
            var newActionId = "6";

            // Act
            cashoutHistory.ChangeTime = newChangeTime;
            cashoutHistory.CashoutAmount = newAmount;
            cashoutHistory.ActionId = newActionId;

            // Assert
            Assert.Equal(newChangeTime, cashoutHistory.ChangeTime);
            Assert.Equal(newAmount, cashoutHistory.CashoutAmount);
            Assert.Equal(newActionId, cashoutHistory.ActionId);
        }

        [Fact]
        public void ChangeTimeText_AfterPropertyChange_ShouldReturnUpdatedFormattedDateTime()
        {
            // Arrange
            var cashoutHistory = new CashoutHistoryDetail(DateTime.Now, 1000m, "1");
            var newChangeTime = new DateTime(2024, 12, 25, 23, 59, 59);

            // Act
            cashoutHistory.ChangeTime = newChangeTime;
            var result = cashoutHistory.ChangeTimeText;

            // Assert
            Assert.Equal("12/25/2024 11:59:59 PM", result);
        }

        [Fact]
        public void CashoutAmountText_AfterPropertyChange_ShouldReturnUpdatedFormattedAmount()
        {
            // Arrange
            var cashoutHistory = new CashoutHistoryDetail(DateTime.Now, 1000m, "1");

            // Act
            cashoutHistory.CashoutAmount = 9999.99m;
            var result = cashoutHistory.CashoutAmountText;

            // Assert
            Assert.Equal("9,999.99", result);
        }

        [Fact]
        public void ActionName_AfterPropertyChange_ShouldReturnUpdatedActionName()
        {
            // Arrange
            var cashoutHistory = new CashoutHistoryDetail(DateTime.Now, 1000m, "1");

            // Act
            cashoutHistory.ActionId = "8";
            var result = cashoutHistory.ActionName;

            // Assert
            Assert.Equal("Cash Out Recovered ", result);
        }
    }
} 