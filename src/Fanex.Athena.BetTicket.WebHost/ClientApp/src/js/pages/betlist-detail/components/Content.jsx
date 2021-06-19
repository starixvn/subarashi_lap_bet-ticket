import React, { useState, useEffect } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../actions';

import Site from '@jsroot/lib/site';
import DataService from '@jsroot/lib/DataService';
import LangHelper from '@jsroot/lib/langHelper';
import { formatDateString } from '@jsroot/lib/dateUtils';

import AppData from '@jsroot/lib/AppData';
import DataGrid from './DataGrid';
import BetListLayout from '@jsroot/layouts/BetListLayout';
import { HeaderText } from '../lib/constants';
import QueryString from '@jsroot/lib/QueryString'

const defaultPagingOption = {
    PageIndex: 1,
    PageSize: 50,
    TotalItem: 0,
    PageList: [50, 100, 200, 300, 400, 500, 1000, 2000]
};

const usePagingOption = (initialValue = defaultPagingOption) => {
    const [pagingOption, setPagingOption] = useState({ ...initialValue });
    const handleChangePaging = (pageIndex, pageSize) => {
        setPagingOption({ ...pagingOption, PageSize: pageSize, PageIndex: pageIndex });
    }

    return {
        pagingOption,
        setPagingOption,
        onChangePaging: handleChangePaging
    }
}

export const Content = (props) => {

    const { requestParam, setRequestParam } = props;
    const [gridData, setGridData] = useState({});
    const { pagingOption, setPagingOption, onChangePaging } = usePagingOption();

    const handleSort = (sortBy, sortOrder) => {
        setRequestParam({ ...requestParam, sortBy, sortOrder });
    }

    useEffect(() => {
        let url = Site.resolveClientUrl(AppData.apiUrl);
        url = QueryString.setParam(url, 'language', window.PageLanguage);
        props.toggleLoader(true);
        DataService.postData(url, requestParam, AppData.access_token)
            .then(response => {
                const result = response.data;
                setGridData({ ...gridData, originalData: result.Data , total: result.Total });
                setPagingOption({ ...pagingOption, TotalItem: result.TotalRows });
                props.toggleLoader(false);
            })
            .catch(error => {
                console.log(error.message);
                setGridData({});
                setPagingOption({ PageSize: 50, PageIndex: 1, TotalItem: 0 });
                props.toggleLoader(false);
            });
    }, [requestParam]);

    useEffect(() => {
            if(gridData && gridData.originalData) {
                const { PageIndex, PageSize } = pagingOption;
                const startIndex = (PageIndex - 1)* PageSize;
                const endIndex = startIndex + PageSize;
                const pagingData = gridData.originalData.slice(startIndex, endIndex);
                setGridData({ ...gridData, data: pagingData });
            }
    }, [pagingOption]);

    const getHeaderName = (column) => {
        if (column === 'WinLoss') {
            return AppData.isAllowedCustomerComm ? `${HeaderText.WinLoss}/${HeaderText.Comm}` : HeaderText.WinLoss;
        }

        return HeaderText[column];
    }

    if (!gridData) {
        return null;
    }

    const dataGridProps = {
        data: gridData.data,
        total: gridData.total,
        pagingOption,
        onPaging: onChangePaging,
        getResource: LangHelper.getSingleResource,
        totalText: LangHelper.getSingleResource('GrandTotal'),
        getHeaderName,
        onSort: handleSort
    }

    return (
        <BetListLayout
            pageName='BetTicket-Detail'
            rootUrl={Site.getRootUrl()}
            getResourceFunc={dataGridProps.getResource}
            formatDateStringFunc={formatDateString}
            baseCurrency={requestParam.baseCurrency}
            beforeShowFunc={() => props.toggleLoader(true)}
            afterShowFunc={() => props.toggleLoader(false)}>
            <DataGrid {...dataGridProps} />
        </BetListLayout>
    )
}

function mapDispatchToProps(dispatch) {
    return bindActionCreators(ActionCreators, dispatch);
}
function mapStateToProps(state) {
    const { requestParam } = state;
    return Object.assign({}, { requestParam });
};

export default connect(mapStateToProps, mapDispatchToProps)(Content);