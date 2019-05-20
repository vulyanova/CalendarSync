import { connect } from 'react-redux';
import Configure from '../components/Configure';
import {
    getCalendars,
    sendConfigurations
} from '../actions';

const mapStateToProps = (state) => ({
    authorizationParams: state.authorizationParams,
    configurations: state.configurations
})

const mapDispatchToProps = dispatch => {
    return {
        getCalendars: (authorizationParams) => dispatch(getCalendars(authorizationParams)),
        sendConfigurations: (configurations) => dispatch(sendConfigurations(configurations))
    }
}

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(Configure)