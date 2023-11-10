import React from 'react';
import { connect } from 'react-redux';
import AppLayout from '@jsroot/layouts/AppLayout';
import Loader from '@jsroot/components/Loader';
import Content from './Content';
import Title from './Title';
import useMobileVersion from '@jsroot/lib/useMobileVersion';

export const AppContainer = ({ isLoading }) => {
    const isUseMobileVersion = useMobileVersion();
    const className = isUseMobileVersion ? 'mobile-container' : '';
    return (
        <AppLayout className={className}>
            <Loader isLoading={isLoading} />
            <Title />
            <Content />
        </AppLayout>
    )
}

function mapStateToProps(state) {
    return Object.assign({}, { isLoading: state.isLoading });
}

export default connect(mapStateToProps)(AppContainer);