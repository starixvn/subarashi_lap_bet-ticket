using System;
using System.Collections.Generic;
using System.Linq;
using Fanex.Athena.BetTicket.Detail.ViewModels;
using Xunit;

namespace Fanex.Athena.BetTicket.Detail.UnitTests.ViewModels
{
    public class BetListDetailViewModelTests
    {
        [Fact]
        public void Constructor_ShouldInitializeSuccessfully()
        {
            // Arrange & Act
            var viewModel = new BetListDetailViewModel();

            // Assert
            Assert.NotNull(viewModel);
        }

        [Fact]
        public void ApiUrl_ShouldBeSettableAndGettable()
        {
            // Arrange
            var viewModel = new BetListDetailViewModel();
            var expectedApiUrl = "/api/betlist/data";

            // Act
            viewModel.ApiUrl = expectedApiUrl;

            // Assert
            Assert.Equal(expectedApiUrl, viewModel.ApiUrl);
        }

        [Fact]
        public void HiddenColumns_ShouldBeSettableAndGettable()
        {
            // Arrange
            var viewModel = new BetListDetailViewModel();
            var expectedHiddenColumns = new[] { 1, 3, 5 };

            // Act
            viewModel.HiddenColumns = expectedHiddenColumns;

            // Assert
            Assert.Equal(expectedHiddenColumns, viewModel.HiddenColumns);
        }

        [Fact]
        public void ShowSummary_ShouldBeSettableAndGettable()
        {
            // Arrange
            var viewModel = new BetListDetailViewModel();

            // Act & Assert - Test true
            viewModel.ShowSummary = true;
            Assert.True(viewModel.ShowSummary);

            // Act & Assert - Test false
            viewModel.ShowSummary = false;
            Assert.False(viewModel.ShowSummary);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("/api/betlist/data")]
        [InlineData("https://example.com/api/betlist")]
        [InlineData("relative/path/to/api")]
        public void ApiUrl_WithVariousValues_ShouldBeSet(string apiUrl)
        {
            // Arrange
            var viewModel = new BetListDetailViewModel();

            // Act
            viewModel.ApiUrl = apiUrl;

            // Assert
            Assert.Equal(apiUrl, viewModel.ApiUrl);
        }

        [Fact]
        public void HiddenColumns_WithNullValue_ShouldBeSet()
        {
            // Arrange
            var viewModel = new BetListDetailViewModel();

            // Act
            viewModel.HiddenColumns = null;

            // Assert
            Assert.Null(viewModel.HiddenColumns);
        }

        [Fact]
        public void HiddenColumns_WithEmptyArray_ShouldBeSet()
        {
            // Arrange
            var viewModel = new BetListDetailViewModel();
            var emptyArray = new int[0];

            // Act
            viewModel.HiddenColumns = emptyArray;

            // Assert
            Assert.Equal(emptyArray, viewModel.HiddenColumns);
            Assert.Empty(viewModel.HiddenColumns);
        }

        [Fact]
        public void HiddenColumns_WithSingleElement_ShouldBeSet()
        {
            // Arrange
            var viewModel = new BetListDetailViewModel();
            var singleElementArray = new[] { 5 };

            // Act
            viewModel.HiddenColumns = singleElementArray;

            // Assert
            Assert.Equal(singleElementArray, viewModel.HiddenColumns);
            Assert.Single(viewModel.HiddenColumns);
            Assert.Equal(5, viewModel.HiddenColumns[0]);
        }

        [Fact]
        public void HiddenColumns_WithMultipleElements_ShouldBeSet()
        {
            // Arrange
            var viewModel = new BetListDetailViewModel();
            var multipleElementsArray = new[] { 1, 2, 3, 4, 5 };

            // Act
            viewModel.HiddenColumns = multipleElementsArray;

            // Assert
            Assert.Equal(multipleElementsArray, viewModel.HiddenColumns);
            Assert.Equal(5, viewModel.HiddenColumns.Length);
        }

        [Fact]
        public void HiddenColumns_WithNegativeValues_ShouldBeSet()
        {
            // Arrange
            var viewModel = new BetListDetailViewModel();
            var negativeValuesArray = new[] { -1, -5, -10 };

            // Act
            viewModel.HiddenColumns = negativeValuesArray;

            // Assert
            Assert.Equal(negativeValuesArray, viewModel.HiddenColumns);
        }

        [Fact]
        public void HiddenColumns_WithDuplicateValues_ShouldBeSet()
        {
            // Arrange
            var viewModel = new BetListDetailViewModel();
            var duplicateValuesArray = new[] { 1, 1, 2, 2, 3 };

            // Act
            viewModel.HiddenColumns = duplicateValuesArray;

            // Assert
            Assert.Equal(duplicateValuesArray, viewModel.HiddenColumns);
        }

        [Fact]
        public void AllProperties_WhenSetTogether_ShouldMaintainValues()
        {
            // Arrange
            var viewModel = new BetListDetailViewModel();
            var expectedApiUrl = "/api/betlist/data";
            var expectedHiddenColumns = new[] { 1, 3, 5, 7 };
            var expectedShowSummary = true;

            // Act
            viewModel.ApiUrl = expectedApiUrl;
            viewModel.HiddenColumns = expectedHiddenColumns;
            viewModel.ShowSummary = expectedShowSummary;

            // Assert
            Assert.Equal(expectedApiUrl, viewModel.ApiUrl);
            Assert.Equal(expectedHiddenColumns, viewModel.HiddenColumns);
            Assert.Equal(expectedShowSummary, viewModel.ShowSummary);
        }

        [Fact]
        public void DefaultValues_ShouldBeCorrect()
        {
            // Arrange & Act
            var viewModel = new BetListDetailViewModel();

            // Assert
            Assert.Null(viewModel.ApiUrl);
            Assert.Null(viewModel.HiddenColumns);
            Assert.False(viewModel.ShowSummary); // Default bool value
        }

        [Fact]
        public void MultipleInstances_ShouldHaveIndependentValues()
        {
            // Arrange
            var viewModel1 = new BetListDetailViewModel();
            var viewModel2 = new BetListDetailViewModel();

            // Act
            viewModel1.ApiUrl = "/api/betlist/data1";
            viewModel1.HiddenColumns = new[] { 1, 2 };
            viewModel1.ShowSummary = true;

            viewModel2.ApiUrl = "/api/betlist/data2";
            viewModel2.HiddenColumns = new[] { 3, 4 };
            viewModel2.ShowSummary = false;

            // Assert
            Assert.Equal("/api/betlist/data1", viewModel1.ApiUrl);
            Assert.Equal(new[] { 1, 2 }, viewModel1.HiddenColumns);
            Assert.True(viewModel1.ShowSummary);

            Assert.Equal("/api/betlist/data2", viewModel2.ApiUrl);
            Assert.Equal(new[] { 3, 4 }, viewModel2.HiddenColumns);
            Assert.False(viewModel2.ShowSummary);
        }
    }

    public class CashOutBetListDetailViewModelTests
    {
        [Fact]
        public void Constructor_ShouldInitializeSuccessfully()
        {
            // Arrange & Act
            var viewModel = new CashOutBetListDetailViewModel();

            // Assert
            Assert.NotNull(viewModel);
        }

        [Fact]
        public void OriginalTicketText_ShouldBeSettableAndGettable()
        {
            // Arrange
            var viewModel = new CashOutBetListDetailViewModel();
            var expectedTicketText = new TicketText { Index = 1, RefNo = "REF001" };

            // Act
            viewModel.OriginalTicketText = expectedTicketText;

            // Assert
            Assert.Equal(expectedTicketText, viewModel.OriginalTicketText);
        }

        [Fact]
        public void CashOutTicketList_ShouldBeSettableAndGettable()
        {
            // Arrange
            var viewModel = new CashOutBetListDetailViewModel();
            var expectedTicketList = new List<CashOutTicket>
            {
                new CashOutTicket { TransId = 123, RefNo = "REF001" },
                new CashOutTicket { TransId = 456, RefNo = "REF002" }
            };

            // Act
            viewModel.CashOutTicketList = expectedTicketList;

            // Assert
            Assert.Equal(expectedTicketList, viewModel.CashOutTicketList);
            Assert.Equal(2, viewModel.CashOutTicketList.Count());
        }

        [Fact]
        public void CashOutTicketTextList_ShouldBeSettableAndGettable()
        {
            // Arrange
            var viewModel = new CashOutBetListDetailViewModel();
            var expectedTicketTextList = new List<CashOutTicketText>
            {
                new CashOutTicketText { TransTime = "2024-01-01", Stake = "100.00" },
                new CashOutTicketText { TransTime = "2024-01-02", Stake = "200.00" }
            };

            // Act
            viewModel.CashOutTicketTextList = expectedTicketTextList;

            // Assert
            Assert.Equal(expectedTicketTextList, viewModel.CashOutTicketTextList);
            Assert.Equal(2, viewModel.CashOutTicketTextList.Count());
        }

        [Fact]
        public void IsCashOut_ShouldBeSettableAndGettable()
        {
            // Arrange
            var viewModel = new CashOutBetListDetailViewModel();

            // Act & Assert - Test true
            viewModel.IsCashOut = true;
            Assert.True(viewModel.IsCashOut);

            // Act & Assert - Test false
            viewModel.IsCashOut = false;
            Assert.False(viewModel.IsCashOut);
        }

        [Fact]
        public void IsHighlightOriginalTicket_ShouldBeSettableAndGettable()
        {
            // Arrange
            var viewModel = new CashOutBetListDetailViewModel();

            // Act & Assert - Test true
            viewModel.IsHighlightOriginalTicket = true;
            Assert.True(viewModel.IsHighlightOriginalTicket);

            // Act & Assert - Test false
            viewModel.IsHighlightOriginalTicket = false;
            Assert.False(viewModel.IsHighlightOriginalTicket);
        }

        [Fact]
        public void AllProperties_WhenSetTogether_ShouldMaintainValues()
        {
            // Arrange
            var viewModel = new CashOutBetListDetailViewModel();
            var expectedTicketText = new TicketText { Index = 1, RefNo = "REF001" };
            var expectedTicketList = new List<CashOutTicket> { new CashOutTicket { TransId = 123 } };
            var expectedTicketTextList = new List<CashOutTicketText> { new CashOutTicketText { TransTime = "2024-01-01" } };

            // Act
            viewModel.OriginalTicketText = expectedTicketText;
            viewModel.CashOutTicketList = expectedTicketList;
            viewModel.CashOutTicketTextList = expectedTicketTextList;
            viewModel.IsCashOut = true;
            viewModel.IsHighlightOriginalTicket = true;

            // Assert
            Assert.Equal(expectedTicketText, viewModel.OriginalTicketText);
            Assert.Equal(expectedTicketList, viewModel.CashOutTicketList);
            Assert.Equal(expectedTicketTextList, viewModel.CashOutTicketTextList);
            Assert.True(viewModel.IsCashOut);
            Assert.True(viewModel.IsHighlightOriginalTicket);
        }
    }

    public class CashOutTicketTests
    {
        [Fact]
        public void Constructor_ShouldInitializeSuccessfully()
        {
            // Arrange & Act
            var ticket = new CashOutTicket();

            // Assert
            Assert.NotNull(ticket);
        }

        [Fact]
        public void BetType_ShouldBeSettableAndGettable()
        {
            // Arrange
            var ticket = new CashOutTicket();
            var expectedBetType = 1;

            // Act
            ticket.BetType = expectedBetType;

            // Assert
            Assert.Equal(expectedBetType, ticket.BetType);
        }

        [Fact]
        public void TransId_ShouldBeSettableAndGettable()
        {
            // Arrange
            var ticket = new CashOutTicket();
            var expectedTransId = 123456789L;

            // Act
            ticket.TransId = expectedTransId;

            // Assert
            Assert.Equal(expectedTransId, ticket.TransId);
        }

        [Fact]
        public void RefNo_ShouldBeSettableAndGettable()
        {
            // Arrange
            var ticket = new CashOutTicket();
            var expectedRefNo = "REF123456789";

            // Act
            ticket.RefNo = expectedRefNo;

            // Assert
            Assert.Equal(expectedRefNo, ticket.RefNo);
        }

        [Fact]
        public void Status_ShouldBeSettableAndGettable()
        {
            // Arrange
            var ticket = new CashOutTicket();
            var expectedStatus = "Won";

            // Act
            ticket.Status = expectedStatus;

            // Assert
            Assert.Equal(expectedStatus, ticket.Status);
        }

        [Fact]
        public void TransDate_ShouldBeSettableAndGettable()
        {
            // Arrange
            var ticket = new CashOutTicket();
            var expectedTransDate = DateTime.Parse("2024-01-01");

            // Act
            ticket.TransDate = expectedTransDate;

            // Assert
            Assert.Equal(expectedTransDate, ticket.TransDate);
        }

        [Fact]
        public void WinLostDate_ShouldBeSettableAndGettable()
        {
            // Arrange
            var ticket = new CashOutTicket();
            var expectedWinLostDate = DateTime.Parse("2024-01-02");

            // Act
            ticket.WinLostDate = expectedWinLostDate;

            // Assert
            Assert.Equal(expectedWinLostDate, ticket.WinLostDate);
        }

        [Fact]
        public void Currency_ShouldBeSettableAndGettable()
        {
            // Arrange
            var ticket = new CashOutTicket();
            var expectedCurrency = 1;

            // Act
            ticket.Currency = expectedCurrency;

            // Assert
            Assert.Equal(expectedCurrency, ticket.Currency);
        }

        [Fact]
        public void CustId_ShouldBeSettableAndGettable()
        {
            // Arrange
            var ticket = new CashOutTicket();
            var expectedCustId = 12345;

            // Act
            ticket.CustId = expectedCustId;

            // Assert
            Assert.Equal(expectedCustId, ticket.CustId);
        }

        [Fact]
        public void Stake_ShouldBeSettableAndGettable()
        {
            // Arrange
            var ticket = new CashOutTicket();
            var expectedStake = 100.50m;

            // Act
            ticket.Stake = expectedStake;

            // Assert
            Assert.Equal(expectedStake, ticket.Stake);
        }

        [Fact]
        public void Price_ShouldBeSettableAndGettable()
        {
            // Arrange
            var ticket = new CashOutTicket();
            var expectedPrice = 1.85m;

            // Act
            ticket.Price = expectedPrice;

            // Assert
            Assert.Equal(expectedPrice, ticket.Price);
        }

        [Fact]
        public void WinLoss_ShouldBeSettableAndGettable()
        {
            // Arrange
            var ticket = new CashOutTicket();
            var expectedWinLoss = 85.42m;

            // Act
            ticket.WinLoss = expectedWinLoss;

            // Assert
            Assert.Equal(expectedWinLoss, ticket.WinLoss);
        }
    }

    public class CashOutTicketTextTests
    {
        [Fact]
        public void Constructor_ShouldInitializeSuccessfully()
        {
            // Arrange & Act
            var ticketText = new CashOutTicketText();

            // Assert
            Assert.NotNull(ticketText);
        }

        [Fact]
        public void TransTime_ShouldBeSettableAndGettable()
        {
            // Arrange
            var ticketText = new CashOutTicketText();
            var expectedTransTime = "2024-01-01 10:30:00";

            // Act
            ticketText.TransTime = expectedTransTime;

            // Assert
            Assert.Equal(expectedTransTime, ticketText.TransTime);
        }

        [Fact]
        public void Stake_ShouldBeSettableAndGettable()
        {
            // Arrange
            var ticketText = new CashOutTicketText();
            var expectedStake = "100.00";

            // Act
            ticketText.Stake = expectedStake;

            // Assert
            Assert.Equal(expectedStake, ticketText.Stake);
        }

        [Fact]
        public void Price_ShouldBeSettableAndGettable()
        {
            // Arrange
            var ticketText = new CashOutTicketText();
            var expectedPrice = "1.85";

            // Act
            ticketText.Price = expectedPrice;

            // Assert
            Assert.Equal(expectedPrice, ticketText.Price);
        }

        [Fact]
        public void WinLoss_ShouldBeSettableAndGettable()
        {
            // Arrange
            var ticketText = new CashOutTicketText();
            var expectedWinLoss = "85.42";

            // Act
            ticketText.WinLoss = expectedWinLoss;

            // Assert
            Assert.Equal(expectedWinLoss, ticketText.WinLoss);
        }

        [Fact]
        public void Status_ShouldBeSettableAndGettable()
        {
            // Arrange
            var ticketText = new CashOutTicketText();
            var expectedStatus = "Won";

            // Act
            ticketText.Status = expectedStatus;

            // Assert
            Assert.Equal(expectedStatus, ticketText.Status);
        }

        [Fact]
        public void IsHighlight_ShouldBeSettableAndGettable()
        {
            // Arrange
            var ticketText = new CashOutTicketText();

            // Act & Assert - Test true
            ticketText.IsHighlight = true;
            Assert.True(ticketText.IsHighlight);

            // Act & Assert - Test false
            ticketText.IsHighlight = false;
            Assert.False(ticketText.IsHighlight);
        }

        [Fact]
        public void AllProperties_WhenSetTogether_ShouldMaintainValues()
        {
            // Arrange
            var ticketText = new CashOutTicketText();
            var expectedTransTime = "2024-01-01 10:30:00";
            var expectedStake = "100.00";
            var expectedPrice = "1.85";
            var expectedWinLoss = "85.42";
            var expectedStatus = "Won";
            var expectedIsHighlight = true;

            // Act
            ticketText.TransTime = expectedTransTime;
            ticketText.Stake = expectedStake;
            ticketText.Price = expectedPrice;
            ticketText.WinLoss = expectedWinLoss;
            ticketText.Status = expectedStatus;
            ticketText.IsHighlight = expectedIsHighlight;

            // Assert
            Assert.Equal(expectedTransTime, ticketText.TransTime);
            Assert.Equal(expectedStake, ticketText.Stake);
            Assert.Equal(expectedPrice, ticketText.Price);
            Assert.Equal(expectedWinLoss, ticketText.WinLoss);
            Assert.Equal(expectedStatus, ticketText.Status);
            Assert.Equal(expectedIsHighlight, ticketText.IsHighlight);
        }
    }
}
