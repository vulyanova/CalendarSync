import './index.css';
import ReactDOM from 'react-dom';
import App from './containers/appContainer';
import React from 'react';
import configureStore from './configureStore';
import { Provider } from 'react-redux';

const store = configureStore();

ReactDOM.render(
    <Provider store={store}>
        <div style={{ padding: '20px' } }>
            <App />
        </div>
    </Provider>,
    document.getElementById('root'));

