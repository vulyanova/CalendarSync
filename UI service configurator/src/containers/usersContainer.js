import { connect } from 'react-redux';
import Users from '../components/Users';
import {
    getUsers,
    changeUser,
    getConfigsData,
    stopAuthorizing,
    successLoading
} from '../actions';

const mapStateToProps = (state) => ({
    users: state.applicationParams.users,
    usersFetched: state.applicationParams.usersFetched
})

const mapDispatchToProps = dispatch => {
    return {
        getUsers: () => dispatch(getUsers()),
        stopAuthorizing: () => dispatch(stopAuthorizing()),
        successLoading: () => dispatch(successLoading()),
        changeUser: (user) => dispatch(changeUser(user)),
        getConfigsData: (user) => dispatch(getConfigsData(user)),
    }
}

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(Users)