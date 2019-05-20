import { connect } from 'react-redux';
import Users from '../components/Users';
import {
    getUsers,
    changeUser,
    stopAuthorizing,
    getCalendars
} from '../actions';

const mapStateToProps = (state) => ({
    users: state.applicationParams.users,
    usersFetched: state.applicationParams.usersFetched
})

const mapDispatchToProps = dispatch => {
    return {
        getUsers: () => dispatch(getUsers()),
        stopAuthorizing: () => dispatch(stopAuthorizing()),
        changeUser: (user) => dispatch(changeUser(user)),
        getCalendars: (user) => dispatch(getCalendars(user)),
    }
}

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(Users)