using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.BetTicket.Core.Services
{
    public interface ILicenseeService
    {
        Task<bool> CheckCustomerBelongsTo(IReadOnlyDictionary<int, int[]> agentList, int custId, long transId = 0);
    }
}