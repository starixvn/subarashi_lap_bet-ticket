using System.Collections.Generic;

namespace Fanex.Athena.BetTicket.Detail.ViewModels
{
    public class BetListViewModel
    {
        public IEnumerable<TicketText> Main { get; set; }
        public BetListTotal Total { get; set; }
    }

    public class BetListTotal
    {
        public decimal SumWinLoss { get; set; }

        public decimal SumLicenseeWl { get; set; }
    }

    public class TicketText
    {
        public int Index { get; set; }

        public string TransTime { get; set; }

        public string Choice { get; set; }

        public string Odds { get; set; }

        public string Stake { get; set; }

        public string WinLoss { get; set; }

        public string LicenseeWL { get; set; }

        public string Status { get; set; }
    }
}