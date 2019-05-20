import React, { Component } from 'react';
import Loading from './Loading';
import Authorize from '../containers/authorizeContainer';
import Configure from '../containers/configureContainer';
import StartService from '../containers/startServiceContainer';

class App extends Component{
    render()
    {
        if (this.props.isLoading)
            return <Loading />
        if (this.props.isAuthorizing)
            return <Authorize /> 
        if (this.props.isConfiguring)
            return <Configure /> 
        return <StartService /> 
    };
}

export default App;

 