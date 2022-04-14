using Fanex.Athena.BetTicket.Detail.DTO;
using System;

namespace Fanex.Athena.BetTicket.Detail.MetaModels
{
    public class NewCashOutBetListDetailMetaModel : CashOutBetListDetailMetaModel
    {
        public int CustId { get; set; }

        public int CurrencyId { get; set; }

        public string Currency { get; set; }

        public DateTime Date { get; set; }
    }
}