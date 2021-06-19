import React from 'react';
import { connect } from 'react-redux';
import AppLayout from '@jsroot/layouts/AppLayout';
import Loader from '@jsroot/components/Loader';
import Content from './Content';
import Title from './Title';

export const AppContainer = ({ isLoading }) => {
    return (
        <AppLayout>
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