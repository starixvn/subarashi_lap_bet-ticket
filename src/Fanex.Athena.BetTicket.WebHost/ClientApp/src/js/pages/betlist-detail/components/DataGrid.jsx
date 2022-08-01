import React from 'react';
import BetListDataGrid from '@jsroot/components/BetListDataGrid';
import AppData from '@jsroot/lib/AppData';

export const DataGrid = (props) => {
    const { data, total, pagingOption, onPaging, getResource, totalText, getHeaderName, onSort } = props;

    const { columns, columnWidths, columnAlignments } = React.useMemo(() => {

        const columns = ['Index', 'TransTime', 'Choice', 'Odds', 'Stake', 'WinLoss', 'LicenseeWL', 'Status'];
        const columnWidths = [30, 95, 190, 40, 60, 65, 65, 80];
        const columnAlignments = [
            'center-align', 'center-align',
            'right-align', 'right-align', 'right-align', 'right-align', 'right-align',
            'center-align'
        ];

        if (AppData.hiddenColumns && AppData.hiddenColumns.length > 0) {
            const hiddenColumnIndexs = AppData.hiddenColumns.map(hiddenColumn => columns.indexOf(hiddenColumn))
            hiddenColumnIndexs.forEach(columnIndex => {
                columns.splice(columnIndex, 1);
                columnWidths.splice(columnIndex, 1);
                columnAlignments.splice(columnIndex, 1);
            })
        }
        return { columns, columnWidths, columnAlignments }
    }, []);

    const totalColumns = React.useMemo(() => {
        const totalColumns = ['WinLoss', 'LicenseeWL'];
        if (AppData.hiddenColumns && AppData.hiddenColumns.length > 0) {
            const hiddenColumnIndexs = AppData.hiddenColumns.map(hiddenColumn => totalColumns.indexOf(hiddenColumn))
            hiddenColumnIndexs.forEach(columnIndex => {
                totalColumns.splice(columnIndex, 1);
            })
        }
        return totalColumns;
    }, []);

    return (
        <BetListDataGrid
            isPaging={true}
            isShowTotal={AppData.isShowTotal}
            pageOption={pagingOption}
            onPaging={onPaging}
            getHeaderNameFunc={getHeaderName}
            columns={columns}
            columnWidths={columnWidths}
            sortableColumns={['Stake', 'WinLoss', 'LicenseeWL']}
            totalColumns={totalColumns}
            columnAlignments={columnAlignments}
            data={data}
            total={total}
            totalText={totalText}
            adjustedHeight={-20}
            getResourceFunc={getResource}
            onSort={onSort}
        />
    )
}

export default DataGrid;