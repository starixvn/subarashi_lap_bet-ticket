using Fanex.Athena.BetTicket.Core.Models;
using Fanex.Core.Utils;

namespace Fanex.Athena.BetTicket.Detail.MetaModels
{
    public class CashOutBetListDetailMetaModel : IValidationMetaModel, IAgentMetaModel, IShowBetFromIPMetaModel
    {
        public string BaseCurrency { get; set; }

        public long TransId { get; set; }

        public long RefNo { get; set; }

        public int BetType { get; set; }

        public long BetdaqId { get; set; }

        public string AgentListXml { get; set; }

        public bool IsCashOut { get; set; }

        public bool HasPlayerComm { get; set; }

        public bool IsShowIpVN { get; set; }

        public bool IsShowIp { get; set; }

        public bool IsValid => true;
    }
}