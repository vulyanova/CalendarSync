import { connect } from 'react-redux';
import Credentials from '../components/Credentials';
import {
    changeClientId,
    changeClientSecret
} from '../actions'

const mapDispatchToProps = dispatch => {
    return {
        changeClientId: (clientId) => dispatch(changeClientId(clientId)),
        changeClientSecret: (clientSecret) => dispatch(changeClientSecret(clientSecret)),
    }
}

export default connect(
    null,
    mapDispatchToProps
)(Credentials)