using System.Collections.Generic;

namespace Fanex.Athena.BetTicket.Detail.Constants
{
    public static class BetListColumn
    {
        public const int ExternalIdColumn = 1;
        public const int SystemIdColumn = 2;
        public const int WinLostColumn = 7;
        public const int BetEffectiveColumn = 8;
        public const int LicenseeWLColumn = 9;

        public static readonly int[] DefaultHiddenColumnIndexes = new[] { ExternalIdColumn, SystemIdColumn, BetEffectiveColumn };

        public static readonly IReadOnlyDictionary<int, string> ColumnNames = new Dictionary<int, string>
        {
            { LicenseeWLColumn, "LicenseeWL" }
        };
    }
}