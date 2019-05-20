import { connect } from 'react-redux';
import User from '../components/User';
import { changeUser } from '../actions'

const mapDispatchToProps = dispatch => {
    return {
        changeUser: (user) => dispatch(changeUser(user)),
    }
}
export default connect(
    null,
    mapDispatchToProps
)(User)