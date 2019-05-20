import { connect } from 'react-redux';
import StartServise from '../components/StartService';
import {
    startService,
} from '../actions';

const mapStateToProps = (state) => ({
    user: state.configurations.user,
})

const mapDispatchToProps = dispatch => {
    return {
        startService: (user) => dispatch(startService(user)),
    }
}

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(StartServise)