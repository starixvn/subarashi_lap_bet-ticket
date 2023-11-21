import LangHelper from '@jsroot/lib/langHelper';

export const HeaderText = {
    Index: "#",
    TransTime: LangHelper.getSingleResource("TransTime"),
    Choice: LangHelper.getSingleResource("Choice"),
    Odds: LangHelper.getSingleResource("Odds"),
    Stake: LangHelper.getSingleResource("Stake"),
    WinLoss: LangHelper.getSingleResource("WinLoss"),
    LicenseeWL: `${LangHelper.getSingleResource("LicenseeWL")}/${LangHelper.getSingleResource("Comm")}`,
    Status: LangHelper.getSingleResource("Status"),
    Comm: LangHelper.getSingleResource("Comm"),
    Details: LangHelper.getSingleResource("Details")
}

export const Columns = ['Index', 'TransTime', 'Choice', 'Odds', 'Stake', 'WinLoss', 'LicenseeWL', 'Status'];
export const ColumnWidths = [40, 110, 160, 60, 70, 70, 70, 85];
export const ColumnAlignments = [
    'center-align', 'center-align',
    'right-align', 'right-align', 'right-align', 'right-align', 'right-align',
    'center-align'
]