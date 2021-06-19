using System;
using System.Collections.Generic;

namespace Fanex.Athena.BetTicket.Detail.ViewModels
{
    public class CashOutBetListDetailViewModel
    {
        public TicketText OriginalTicketText { get; set; }

        public IEnumerable<CashOutTicket> CashOutTicketList { get; set; }

        public IEnumerable<CashOutTicketText> CashOutTicketTextList { get; set; }

        public bool IsCashOut { get; set; }

        public bool IsHighlightOriginalTicket { get; set; }
    }

    public class CashOutTicket
    {
        public int BetType { get; set; }

        public long TransId { get; set; }

        public string RefNo { get; set; }

        public string Status { get; set; }

        public DateTime TransDate { get; set; }

        public DateTime WinLostDate { get; set; }

        public int Currency { get; set; }

        public int CustId { get; set; }

        public decimal Stake { get; set; }

        public decimal Price { get; set; }

        public decimal WinLoss { get; set; }
    }

    public class CashOutTicketText
    {
        public string TransTime { get; set; }

        public string Stake { get; set; }

        public string Price { get; set; }

        public string WinLoss { get; set; }

        public string Status { get; set; }

        public bool IsHighlight { get; set; }
    }
}