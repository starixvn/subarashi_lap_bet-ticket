using Fanex.Athena.BetTicket.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.BetTicket.Infrastructure.Services
{
    public class LicenseeService : ILicenseeService
    {
        private readonly IDbFactory _dbFactory;

        public LicenseeService(IDbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<bool> CheckCustomerBelongsTo(IReadOnlyDictionary<int, int[]> agentList, int custId, long transId = 0)
        {
            if (custId == 0 && transId == 0)
            {
                return true;
            }

            using (var db = _dbFactory.GetObjectDb("Lrf_CheckBelonging_GetAgentInfo"))
            {
                db.SetParameterValue("transid", transId);
                db.SetParameterValue("custid", custId);

                using (var reader = await db.ExecuteReaderAsync())
                {
                    if (reader.Read())
                    {
                        var actualCurrencyId = Convert.ToInt32(reader["CurrencyId"]);
                        var agentId = Convert.ToInt32(reader["CustId"]);
                        return agentList.Any(i => i.Key == actualCurrencyId && i.Value.Contains(agentId));
                    }
                }
            }
            return false;
        }
    }
}