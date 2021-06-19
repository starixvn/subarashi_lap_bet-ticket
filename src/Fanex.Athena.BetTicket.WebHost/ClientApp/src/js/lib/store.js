import { createStore, applyMiddleware, compose } from 'redux'
import thunkMiddleware from 'redux-thunk'
import { createLogger } from 'redux-logger'

const debuggingConfig = {
    name: "__lrfConfig__",
    env: "local"
}

const isDev = process.env.NODE_ENV === "development";
const configName = debuggingConfig.name;

const loggerMiddleware = createLogger({
    predicate: (getState, action) => isDev ||
        (window[configName] &&
            window[configName].env === debuggingConfig.env &&
            (!window[configName].actionFilter || window[configName].actionFilter === action.type))
});

export default class Store {
    static createStore(reducer, initialState) {
        const enhancer = compose(
            applyMiddleware(
                thunkMiddleware, // lets us dispatch() functions
                loggerMiddleware
            )
        );

        return createStore(reducer, initialState, enhancer);
    }
}
