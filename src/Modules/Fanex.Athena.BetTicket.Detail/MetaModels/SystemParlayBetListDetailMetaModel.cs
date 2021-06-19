using Fanex.Core.Utils;

namespace Fanex.Athena.BetTicket.Detail.MetaModels
{
    public class SystemParlayBetListDetailMetaModel : IValidationMetaModel, IAgentMetaModel
    {
        public long RefNo { get; set; }
        public string BaseCurrency { get; set; }
        public string AgentListXml { get; set; }
        public bool IsValid => RefNo > 0;
    }
}