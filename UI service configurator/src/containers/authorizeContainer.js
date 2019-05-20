import { connect } from 'react-redux';
import Authorize from '../components/Authorize';
import {
    authorize
} from '../actions';

const mapStateToProps = (state) => ({
    authorizationParams: state.authorizationParams
})

const mapDispatchToProps = dispatch => {
    return {
        authorize: (authorizationParams) => dispatch(authorize(authorizationParams))
    }
}

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(Authorize)