using Fanex.Athena.BetTicket.Infrastructure.Models;
using Fanex.Core.Utils;

namespace Fanex.Athena.BetTicket.Detail.MetaModels
{
    public class ColossusBetListDetailMetaModel : IValidationMetaModel, IAgentMetaModel, ICustomerIdentityMetaModel
    {
        public int UserId { get; set; }

        public int CustId { get; set; }

        public long RefNo { get; set; }

        public string BaseCurrency { get; set; }

        public long BetCheck { get; set; }

        public string AgentListXml { get; set; }
        public long TransId { get; set; } = 0;

        public bool IsValid => CustId > 0;

    }
}