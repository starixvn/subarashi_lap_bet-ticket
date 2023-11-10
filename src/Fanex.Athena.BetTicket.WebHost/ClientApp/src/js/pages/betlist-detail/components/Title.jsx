import React from 'react';
import { connect } from 'react-redux';
import AppData from '@jsroot/lib/AppData';
import LangHelper from '@jsroot/lib/langHelper';
import Site from '@jsroot/lib/site';
import DataService from '@jsroot/lib/DataService';
import QueryString from '@jsroot/lib/QueryString'
import { bindActionCreators } from 'redux';
import { ActionCreators } from '../actions';
import { saveAs } from 'file-saver';

export const Title = (props) => {
    const handleExportExcel = () => {
        const { requestParam, toggleLoader } = props;
        let url = Site.resolveClientUrl(AppData.exportUrl);
        url = QueryString.setParam(url, 'language', window.PageLanguage);
        toggleLoader(true);
        DataService.getFile(url, requestParam, AppData.access_token)
            .then(response => {
                const NoContentStatusCode = 204;

                if (response.status === NoContentStatusCode) {
                    alert(LangHelper.getSingleResource("NoInfo"));
                } else {
                    const blob = new Blob([response.data], {type: "application/vnd.ms-excel"})
                    saveAs(blob, `${AppData.exportFileName}.xls`);
                }
                toggleLoader(false);
            })
            .catch(error => {
                console.log(error.message);
                toggleLoader(false);
                alert(LangHelper.getSingleResource("NoInfo"));
            });
    }

    return (
        <div class="report-title-container">
            <span className="caret-block">
                <span className="icon-excel-wrapper" onClick={handleExportExcel}>
                    <i className="icon-excel" title={LangHelper.getSingleResource('ExportToExcel')}></i>
                </span>
            </span>
            <span>{AppData.reportTitle}</span>
        </div>
    )
}

function mapDispatchToProps(dispatch) {
    const { toggleLoader } = ActionCreators;

    return bindActionCreators({ toggleLoader }, dispatch);
}

function mapStateToProps(state) {
    const { requestParam } = state;
    return Object.assign({}, { requestParam });
};

export default connect(mapStateToProps, mapDispatchToProps)(Title);