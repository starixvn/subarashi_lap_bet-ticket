using Fanex.Core.Utils;
using System;

namespace Fanex.Athena.BetTicket.Result.MetaModels
{
    public class SportsResultMetaModel : IValidationMetaModel
    {
        public int MatchId { get; set; }

        public int Bettype { get; set; }

        public int? RaceNo { get; set; }

        public int SportType { get; set; }

        public int? IsOutRight { get; set; }

        public long? RefNo { get; set; }

        public DateTime? WinLossDate { get; set; }

        public long? BetId { get; set; }

        public int CustId { get; set; }

        public int BetCheck { get; set; }

        public int Handicap1 { get; set; }

        public int Handicap2 { get; set; }

        public string Language { get; set; }

        public string TransDesc { get; set; }

        public bool IsValid => true;
    }
}