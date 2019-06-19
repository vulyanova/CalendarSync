import { connect } from 'react-redux';
import Authorize from '../components/Authorize';
import {
    authorize,
    getHistory
} from '../actions';

const mapStateToProps = (state) => ({
    authorizationParams: state.authorizationParams
})

const mapDispatchToProps = dispatch => {
    return {
        authorize: (authorizationParams) => dispatch(authorize(authorizationParams)),
        getHistory: (size, page) => dispatch(getHistory(size, page))
    }
}

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(Authorize)