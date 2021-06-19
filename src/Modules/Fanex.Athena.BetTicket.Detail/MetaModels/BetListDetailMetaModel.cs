using Fanex.Core.Utils;
using System;

namespace Fanex.Athena.BetTicket.Detail.MetaModels
{
    public class BetListDetailMetaModel : IValidationMetaModel, IPlayerComMetaModel
    {
        public bool HasPlayerCom { get; set; }

        public string BaseCurrency { get; set; }
        public DateTime Date { get; set; }
        public int CustId { get; set; }
        public long TransId { get; set; }
        public long TransType { get; set; }
        public long RefCode { get; set; }
        public bool IsValid => true;
    }
}