using System.Threading.Tasks;
using Fanex.Athena.BetTicket.UnitTests.Common.AutoData;
using Fanex.Data;
using Moq;
using Xunit;

namespace Fanex.Athena.BetTicket.Core.UnitTests.Services
{
    public class ContextServiceTest
    {
        [Theory, AutoMoqData]
        public async Task AngentList_Null_ReturnDefault(
            Mock<IObjectDb> objDb) { }
    }
}
