using Fanex.Athena.BetTicket.Detail.ViewModels;
using Xunit;

namespace Fanex.Athena.BetTicket.Detail.UnitTests.ViewModels
{
    public class TicketTextTests
    {
        [Fact]
        public void Constructor_ShouldInitializeSuccessfully()
        {
            // Arrange & Act
            var ticketText = new TicketText();

            // Assert
            Assert.NotNull(ticketText);
        }

        [Fact]
        public void Index_ShouldBeSettableAndGettable()
        {
            // Arrange
            var ticketText = new TicketText();
            var expectedIndex = 123;

            // Act
            ticketText.Index = expectedIndex;

            // Assert
            Assert.Equal(expectedIndex, ticketText.Index);
        }

        [Fact]
        public void TransTime_ShouldBeSettableAndGettable()
        {
            // Arrange
            var ticketText = new TicketText();
            var expectedTransTime = "2024-01-01 10:30:00";

            // Act
            ticketText.TransTime = expectedTransTime;

            // Assert
            Assert.Equal(expectedTransTime, ticketText.TransTime);
        }

        [Fact]
        public void Choice_ShouldBeSettableAndGettable()
        {
            // Arrange
            var ticketText = new TicketText();
            var expectedChoice = "Team A vs Team B - Over 2.5";

            // Act
            ticketText.Choice = expectedChoice;

            // Assert
            Assert.Equal(expectedChoice, ticketText.Choice);
        }

        [Fact]
        public void Odds_ShouldBeSettableAndGettable()
        {
            // Arrange
            var ticketText = new TicketText();
            var expectedOdds = "1.85";

            // Act
            ticketText.Odds = expectedOdds;

            // Assert
            Assert.Equal(expectedOdds, ticketText.Odds);
        }

        [Fact]
        public void Stake_ShouldBeSettableAndGettable()
        {
            // Arrange
            var ticketText = new TicketText();
            var expectedStake = "100.00";

            // Act
            ticketText.Stake = expectedStake;

            // Assert
            Assert.Equal(expectedStake, ticketText.Stake);
        }

        [Fact]
        public void WinLoss_ShouldBeSettableAndGettable()
        {
            // Arrange
            var ticketText = new TicketText();
            var expectedWinLoss = "85.00";

            // Act
            ticketText.WinLoss = expectedWinLoss;

            // Assert
            Assert.Equal(expectedWinLoss, ticketText.WinLoss);
        }

        [Fact]
        public void LicenseeWL_ShouldBeSettableAndGettable()
        {
            // Arrange
            var ticketText = new TicketText();
            var expectedLicenseeWL = "75.00";

            // Act
            ticketText.LicenseeWL = expectedLicenseeWL;

            // Assert
            Assert.Equal(expectedLicenseeWL, ticketText.LicenseeWL);
        }

        [Fact]
        public void Status_ShouldBeSettableAndGettable()
        {
            // Arrange
            var ticketText = new TicketText();
            var expectedStatus = "Won";

            // Act
            ticketText.Status = expectedStatus;

            // Assert
            Assert.Equal(expectedStatus, ticketText.Status);
        }

        [Fact]
        public void RefNo_ShouldBeSettableAndGettable()
        {
            // Arrange
            var ticketText = new TicketText();
            var expectedRefNo = "REF123456789";

            // Act
            ticketText.RefNo = expectedRefNo;

            // Assert
            Assert.Equal(expectedRefNo, ticketText.RefNo);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void Index_WithVariousValues_ShouldBeSet(int index)
        {
            // Arrange
            var ticketText = new TicketText();

            // Act
            ticketText.Index = index;

            // Assert
            Assert.Equal(index, ticketText.Index);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("2024-01-01 10:30:00")]
        [InlineData("Invalid Date")]
        public void TransTime_WithVariousValues_ShouldBeSet(string transTime)
        {
            // Arrange
            var ticketText = new TicketText();

            // Act
            ticketText.TransTime = transTime;

            // Assert
            Assert.Equal(transTime, ticketText.TransTime);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("Team A vs Team B - Over 2.5")]
        [InlineData("Single Match Choice")]
        public void Choice_WithVariousValues_ShouldBeSet(string choice)
        {
            // Arrange
            var ticketText = new TicketText();

            // Act
            ticketText.Choice = choice;

            // Assert
            Assert.Equal(choice, ticketText.Choice);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("1.85")]
        [InlineData("0.00")]
        [InlineData("-1.50")]
        [InlineData("Invalid Odds")]
        public void Odds_WithVariousValues_ShouldBeSet(string odds)
        {
            // Arrange
            var ticketText = new TicketText();

            // Act
            ticketText.Odds = odds;

            // Assert
            Assert.Equal(odds, ticketText.Odds);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("100.00")]
        [InlineData("0.00")]
        [InlineData("-50.00")]
        [InlineData("Invalid Stake")]
        public void Stake_WithVariousValues_ShouldBeSet(string stake)
        {
            // Arrange
            var ticketText = new TicketText();

            // Act
            ticketText.Stake = stake;

            // Assert
            Assert.Equal(stake, ticketText.Stake);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("85.00")]
        [InlineData("0.00")]
        [InlineData("-100.00")]
        [InlineData("Invalid WinLoss")]
        public void WinLoss_WithVariousValues_ShouldBeSet(string winLoss)
        {
            // Arrange
            var ticketText = new TicketText();

            // Act
            ticketText.WinLoss = winLoss;

            // Assert
            Assert.Equal(winLoss, ticketText.WinLoss);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("75.00")]
        [InlineData("0.00")]
        [InlineData("-25.00")]
        [InlineData("Invalid LicenseeWL")]
        public void LicenseeWL_WithVariousValues_ShouldBeSet(string licenseeWL)
        {
            // Arrange
            var ticketText = new TicketText();

            // Act
            ticketText.LicenseeWL = licenseeWL;

            // Assert
            Assert.Equal(licenseeWL, ticketText.LicenseeWL);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("Won")]
        [InlineData("Lost")]
        [InlineData("Pending")]
        [InlineData("Void")]
        public void Status_WithVariousValues_ShouldBeSet(string status)
        {
            // Arrange
            var ticketText = new TicketText();

            // Act
            ticketText.Status = status;

            // Assert
            Assert.Equal(status, ticketText.Status);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("REF123456789")]
        [InlineData("SHORT")]
        [InlineData("VERY_LONG_REFERENCE_NUMBER_123456789")]
        public void RefNo_WithVariousValues_ShouldBeSet(string refNo)
        {
            // Arrange
            var ticketText = new TicketText();

            // Act
            ticketText.RefNo = refNo;

            // Assert
            Assert.Equal(refNo, ticketText.RefNo);
        }

        [Fact]
        public void AllProperties_WhenSetTogether_ShouldMaintainValues()
        {
            // Arrange
            var ticketText = new TicketText();
            var expectedIndex = 123;
            var expectedTransTime = "2024-01-01 10:30:00";
            var expectedChoice = "Team A vs Team B - Over 2.5";
            var expectedOdds = "1.85";
            var expectedStake = "100.00";
            var expectedWinLoss = "85.00";
            var expectedLicenseeWL = "75.00";
            var expectedStatus = "Won";
            var expectedRefNo = "REF123456789";

            // Act
            ticketText.Index = expectedIndex;
            ticketText.TransTime = expectedTransTime;
            ticketText.Choice = expectedChoice;
            ticketText.Odds = expectedOdds;
            ticketText.Stake = expectedStake;
            ticketText.WinLoss = expectedWinLoss;
            ticketText.LicenseeWL = expectedLicenseeWL;
            ticketText.Status = expectedStatus;
            ticketText.RefNo = expectedRefNo;

            // Assert
            Assert.Equal(expectedIndex, ticketText.Index);
            Assert.Equal(expectedTransTime, ticketText.TransTime);
            Assert.Equal(expectedChoice, ticketText.Choice);
            Assert.Equal(expectedOdds, ticketText.Odds);
            Assert.Equal(expectedStake, ticketText.Stake);
            Assert.Equal(expectedWinLoss, ticketText.WinLoss);
            Assert.Equal(expectedLicenseeWL, ticketText.LicenseeWL);
            Assert.Equal(expectedStatus, ticketText.Status);
            Assert.Equal(expectedRefNo, ticketText.RefNo);
        }
    }
}
