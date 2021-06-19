using System;
using System.Collections.Generic;

namespace Fanex.Athena.BetTicket.Detail.ViewModels
{
    public class ColossusBetListDetailViewModel
    {
        public IEnumerable<ColossusBetDetail> DataList { get; set; }

        public decimal TotalSoldPercent { get; set; }

        public decimal TotalWinLoss { get; set; }

        public decimal TotalComm { get; set; }

        public decimal TotalSoldWinLoss { get; set; }

        public string IframeUrl { get; set; }
    }

    public class ColossusBetDetail
    {
        public DateTime TransDate { get; set; }

        public string TransactionType { get; set; }

        public string WinLoss { get; set; }

        public decimal Commission { get; set; }
    }
}