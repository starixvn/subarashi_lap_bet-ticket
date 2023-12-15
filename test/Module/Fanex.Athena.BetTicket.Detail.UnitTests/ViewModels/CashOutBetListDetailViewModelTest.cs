using Fanex.Athena.BetTicket.Detail.ViewModels;
using Fanex.Athena.BetTicket.UnitTests.Common;
using Xunit;

namespace Fanex.Athena.BetTicket.Detail.UnitTests.ViewModels
{
    public class CashOutBetListDetailViewModelTest
    {
        [Fact]
        public void DetailHistory_CashOutExtraInfo_ReturnCashOutHistoryDetailList()
        {
            var viewModel = FakeHelper.Create<NewCashOutBetListDetailViewModel>();
            viewModel.CashoutExtraInfo = @"<root>
	                                        <item ct='2011-03-21 13:26' w='1' s='1'></item>
	                                        <item ct='2011-03-21 13:26' w='1' s='6'></item>
	                                        <item ct='2011-03-21 13:26' w='1' s='8'></item>
                                          </root>";

            var result = viewModel.DetailHistory;

            Assert.NotEmpty(result);
            Assert.Equal("1", result[0].ActionId);
            Assert.Equal("6", result[1].ActionId);
            Assert.Equal("8", result[2].ActionId);
        }
    }
}
