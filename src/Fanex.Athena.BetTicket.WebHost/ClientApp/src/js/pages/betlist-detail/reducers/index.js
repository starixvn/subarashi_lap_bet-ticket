import { combineReducers } from 'redux';
import * as loaderReducer from '../../../reducers/loaderReducer';
import * as requestParamReducer from '../../../reducers/requestParamReducer';

export default combineReducers(Object.assign({},
    loaderReducer,
    requestParamReducer
));