using Fanex.Athena.BetTicket.Detail.DTO;
using Fanex.Athena.BetTicket.UnitTests.Common;
using Xunit;

namespace Fanex.Athena.BetTicket.Detail.UnitTests.DTO
{
    public class CashOutHistoryDetailTest
    {
        [Fact]
        public void ActionName_ValidActionId_ReturnResource()
        {
            var cashOutHistoryDetail = FakeHelper.Create<CashoutHistoryDetail>(x => x.ActionId, "1");

            var result = cashOutHistoryDetail.ActionName;

            Assert.NotEqual("1", result);
        }

        [Fact]
        public void ActionName_InValidActionId_ReturnActionId()
        {
            var cashOutHistoryDetail = FakeHelper.Create<CashoutHistoryDetail>(x => x.ActionId, "100");

            var result = cashOutHistoryDetail.ActionName;

            Assert.Equal("100", result);
        }
    }
}
