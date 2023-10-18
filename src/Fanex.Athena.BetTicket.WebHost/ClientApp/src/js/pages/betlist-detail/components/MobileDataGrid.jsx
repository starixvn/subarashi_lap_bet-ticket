import React, { useEffect, useRef, useState, useMemo } from 'react';
import PropTypes from 'prop-types';
import { Table, HeaderRow, HeaderCell, Row, Cell } from '@legion-lib/react-grid';
import NoDataTitle from '@jsroot/components/NoDataTitle';
import { useElementSize, useWindowSize } from 'usehooks-ts';
import { RefPager } from '@jsroot/components/RefPager';
import { v4 as uuidv4 } from 'uuid';
import MobileBetlistContent from './MobileBetlistContent';
import MobileWinLossCellContent from './MobileWinLossCellContent';

const SmallColumnWidths = [50, 17, 20, 13]
const LargeColumnWidths = ['53%', '15%', '22%', '10%']
const Columns = ['Details', 'Stake', 'WinLoss'];

export const MobileDataGrid = (props) => {
    const { data, total, pagingOption, onPaging, totalText, getHeaderName } = props;
    const ref = useRef(null);
    const [id, setId] = useState(`${uuidv4()}`);
    const [pagerRef, pagerSize] = useElementSize();
    const { width } = useWindowSize();
    const collumnWidths = width > 400 ? LargeColumnWidths : SmallColumnWidths

    useEffect(() => {
        setId(`${uuidv4()}`);
        ref.current?.scrollIntoView();
    }, [data]);

    useEffect(() => {
        const style = `bottom: ${pagerSize.height - 1}px`;
        ref.current?.getElementsByClassName('footer-content')[0]?.setAttribute('style', style);
    }, [pagerSize?.height, id])

    const buildHeader = () => {
        let rows = [];
        for (let i = 0; i < Columns.length; i++) {
            const column = Columns[i];
            const headerName = getHeaderName(column);

            rows.push(
                <HeaderCell key={column} colWidth={collumnWidths[i]} >{headerName}</HeaderCell>
            )
        }
        return <HeaderRow style={{ height: 32 }}>{rows}</HeaderRow>;
    }
    const buildBody = () => {
        if (!data) {
            return null;
        }
        return data.map((item) => {
            return <Row key={item.RefNo}>
                <Cell><MobileBetlistContent item={item} /></Cell>
                <Cell className="right-align" dangerouslySetInnerHTML={{ __html: item.Stake }}></Cell>
                <Cell className="right-align"><MobileWinLossCellContent item={item} /></Cell>
            </Row>
        })
    }

    const buildFooter = () => {
        if (!total) {
            return null;
        }
        return (

            <Row className="fixed-table-grant-total">
                <Cell colSpan={2}>{totalText}</Cell>
                <Cell className="right-align" dangerouslySetInnerHTML={{ __html: total.WinLoss }}></Cell>
            </Row>)
    }

    const header = useMemo(() => buildHeader(), [width]);
    const body = useMemo(() => buildBody(), [data]);
    const footer = useMemo(() => buildFooter(), [total]);
    if (!data?.length) {
        return <NoDataTitle />;
    }

    const isShowPagingInfo = pagerSize.width > 400;

    return (
        <div className="mobile-betlist-container betlist-container result-content" ref={ref}>
            <Table key={id}
                autoWidth={true}
                autoHeight={false}
                header={header}
                body={body}
                footer={footer}
                isPaging={false}
                bodyHeight="100%"
                containerPadding={30}
                isIncludeScrollWidth={false} />
            <RefPager ref={pagerRef} onPaging={onPaging} pageOption={pagingOption} isShowPagingInfo={isShowPagingInfo} />
        </div>
    );
};

const propTypes = {
    dataUrl: PropTypes.string,
    isPaging: PropTypes.bool,
    header: PropTypes.oneOfType([PropTypes.array, PropTypes.element, PropTypes.object]),
    body: PropTypes.oneOfType([PropTypes.array, PropTypes.element, PropTypes.object]),
    footer: PropTypes.oneOfType([PropTypes.array, PropTypes.element, PropTypes.object]),
    columns: PropTypes.arrayOf(PropTypes.string)
};
const defaultProps = {
    isPaging: false
};

MobileDataGrid.propTypes = propTypes;
MobileDataGrid.defaultProps = defaultProps;

export default MobileDataGrid