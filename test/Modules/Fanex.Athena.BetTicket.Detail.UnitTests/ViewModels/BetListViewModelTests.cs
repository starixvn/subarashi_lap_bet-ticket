using System.Collections.Generic;
using System.Linq;
using Fanex.Athena.BetTicket.Detail.ViewModels;
using Xunit;

namespace Fanex.Athena.BetTicket.Detail.UnitTests.ViewModels
{
    public class BetListViewModelTests
    {
        [Fact]
        public void Constructor_ShouldInitializeSuccessfully()
        {
            // Arrange & Act
            var viewModel = new BetListViewModel();

            // Assert
            Assert.NotNull(viewModel);
        }

        [Fact]
        public void Main_ShouldBeSettableAndGettable()
        {
            // Arrange
            var viewModel = new BetListViewModel();
            var expectedMain = new List<TicketText>
            {
                new TicketText { Index = 1, RefNo = "REF001" },
                new TicketText { Index = 2, RefNo = "REF002" }
            };

            // Act
            viewModel.Main = expectedMain;

            // Assert
            Assert.Equal(expectedMain, viewModel.Main);
            Assert.Equal(2, viewModel.Main.Count());
        }

        [Fact]
        public void Total_ShouldBeSettableAndGettable()
        {
            // Arrange
            var viewModel = new BetListViewModel();
            var expectedTotal = new BetListTotal
            {
                SumWinLoss = 1000.50m,
                SumLicenseeWl = 500.25m,
                SumStake = 2000.75m,
                SumComm = 100.10m
            };

            // Act
            viewModel.Total = expectedTotal;

            // Assert
            Assert.Equal(expectedTotal, viewModel.Total);
        }

        [Fact]
        public void Main_WithNullValue_ShouldBeSet()
        {
            // Arrange
            var viewModel = new BetListViewModel();

            // Act
            viewModel.Main = null;

            // Assert
            Assert.Null(viewModel.Main);
        }

        [Fact]
        public void Total_WithNullValue_ShouldBeSet()
        {
            // Arrange
            var viewModel = new BetListViewModel();

            // Act
            viewModel.Total = null;

            // Assert
            Assert.Null(viewModel.Total);
        }

        [Fact]
        public void Main_WithEmptyCollection_ShouldBeSet()
        {
            // Arrange
            var viewModel = new BetListViewModel();
            var emptyMain = new List<TicketText>();

            // Act
            viewModel.Main = emptyMain;

            // Assert
            Assert.Equal(emptyMain, viewModel.Main);
            Assert.Empty(viewModel.Main);
        }

        [Fact]
        public void AllProperties_WhenSetTogether_ShouldMaintainValues()
        {
            // Arrange
            var viewModel = new BetListViewModel();
            var expectedMain = new List<TicketText>
            {
                new TicketText { Index = 1, RefNo = "REF001", Stake = "100.00" },
                new TicketText { Index = 2, RefNo = "REF002", Stake = "200.00" }
            };
            var expectedTotal = new BetListTotal
            {
                SumWinLoss = 1000.50m,
                SumLicenseeWl = 500.25m,
                SumStake = 2000.75m,
                SumComm = 100.10m
            };

            // Act
            viewModel.Main = expectedMain;
            viewModel.Total = expectedTotal;

            // Assert
            Assert.Equal(expectedMain, viewModel.Main);
            Assert.Equal(expectedTotal, viewModel.Total);
            Assert.Equal(2, viewModel.Main.Count());
        }
    }

    public class BetListTotalTests
    {
        [Fact]
        public void Constructor_ShouldInitializeSuccessfully()
        {
            // Arrange & Act
            var total = new BetListTotal();

            // Assert
            Assert.NotNull(total);
        }

        [Fact]
        public void SumWinLoss_ShouldBeSettableAndGettable()
        {
            // Arrange
            var total = new BetListTotal();
            var expectedSumWinLoss = 1000.50m;

            // Act
            total.SumWinLoss = expectedSumWinLoss;

            // Assert
            Assert.Equal(expectedSumWinLoss, total.SumWinLoss);
        }

        [Fact]
        public void SumLicenseeWl_ShouldBeSettableAndGettable()
        {
            // Arrange
            var total = new BetListTotal();
            var expectedSumLicenseeWl = 500.25m;

            // Act
            total.SumLicenseeWl = expectedSumLicenseeWl;

            // Assert
            Assert.Equal(expectedSumLicenseeWl, total.SumLicenseeWl);
        }

        [Fact]
        public void SumStake_ShouldBeSettableAndGettable()
        {
            // Arrange
            var total = new BetListTotal();
            var expectedSumStake = 2000.75m;

            // Act
            total.SumStake = expectedSumStake;

            // Assert
            Assert.Equal(expectedSumStake, total.SumStake);
        }

        [Fact]
        public void SumComm_ShouldBeSettableAndGettable()
        {
            // Arrange
            var total = new BetListTotal();
            var expectedSumComm = 100.10m;

            // Act
            total.SumComm = expectedSumComm;

            // Assert
            Assert.Equal(expectedSumComm, total.SumComm);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1000)]
        [InlineData(1000)]
        public void SumWinLoss_WithVariousValues_ShouldBeSet(decimal sumWinLoss)
        {
            // Arrange
            var total = new BetListTotal();

            // Act
            total.SumWinLoss = sumWinLoss;

            // Assert
            Assert.Equal(sumWinLoss, total.SumWinLoss);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-500)]
        [InlineData(500)]
        public void SumLicenseeWl_WithVariousValues_ShouldBeSet(decimal sumLicenseeWl)
        {
            // Arrange
            var total = new BetListTotal();

            // Act
            total.SumLicenseeWl = sumLicenseeWl;

            // Assert
            Assert.Equal(sumLicenseeWl, total.SumLicenseeWl);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-2000)]
        [InlineData(2000)]
        public void SumStake_WithVariousValues_ShouldBeSet(decimal sumStake)
        {
            // Arrange
            var total = new BetListTotal();

            // Act
            total.SumStake = sumStake;

            // Assert
            Assert.Equal(sumStake, total.SumStake);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-100)]
        [InlineData(100)]
        public void SumComm_WithVariousValues_ShouldBeSet(decimal sumComm)
        {
            // Arrange
            var total = new BetListTotal();

            // Act
            total.SumComm = sumComm;

            // Assert
            Assert.Equal(sumComm, total.SumComm);
        }

        [Fact]
        public void AllProperties_WhenSetTogether_ShouldMaintainValues()
        {
            // Arrange
            var total = new BetListTotal();
            var expectedSumWinLoss = 1000.50m;
            var expectedSumLicenseeWl = 500.25m;
            var expectedSumStake = 2000.75m;
            var expectedSumComm = 100.10m;

            // Act
            total.SumWinLoss = expectedSumWinLoss;
            total.SumLicenseeWl = expectedSumLicenseeWl;
            total.SumStake = expectedSumStake;
            total.SumComm = expectedSumComm;

            // Assert
            Assert.Equal(expectedSumWinLoss, total.SumWinLoss);
            Assert.Equal(expectedSumLicenseeWl, total.SumLicenseeWl);
            Assert.Equal(expectedSumStake, total.SumStake);
            Assert.Equal(expectedSumComm, total.SumComm);
        }
    }
}
