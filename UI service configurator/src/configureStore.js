import { createStore, combineReducers, applyMiddleware } from 'redux';
import { authorizationParams } from './reducers/authorizationParams';
import { applicationParams } from './reducers/applicationParams';
import { timers } from './reducers/timers';
import { configurations } from './reducers/configurations';
import createSagaMiddleware from 'redux-saga';
import { initSagas } from './initSagas'

const configureStore = () => {

    const reducers = combineReducers({ authorizationParams, configurations, applicationParams, timers});
    const sagaMiddleware = createSagaMiddleware();

    const store = createStore(
        reducers,
        applyMiddleware(sagaMiddleware)
    );   

    initSagas(sagaMiddleware);

    return store;
}

export default configureStore;