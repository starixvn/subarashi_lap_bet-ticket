using Fanex.Athena.BetTicket.Core.Models;
using Fanex.Core.Utils;

namespace Fanex.Athena.BetTicket.Detail.MetaModels
{
    public class BetListDetailApiMetaModel : IValidationMetaModel, IAgentMetaModel, IPlayerComMetaModel, IShowBetFromIPMetaModel, ICustomerIdentityMetaModel
    {
        public BetListDetailApiMetaModel()
        {
            PageIndex = 1;
            PageSize = int.MaxValue - 1;
        }

        public string AgentListXml { get; set; }

        public string BaseCurrency { get; set; }

        public bool HasPlayerCom { get; set; }

        public string UspKey { get; set; }

        public string SortBy { get; set; }

        public string SortOrder { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int CustId { get; set; }

        public long TransId { get; set; }

        public long TransType { get; set; }

        public long RefCode { get; set; }

        public bool IsShowIp { get; set; }

        public bool IsShowIpVN { get; set; }

        public bool IsMobileView { get; set; }

        public bool IsValid => true;

    }
}