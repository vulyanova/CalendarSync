import { connect } from 'react-redux';
import App from '../components/App'

const mapStateToProps = (state) => ({   
    isLoading: state.applicationParams.isLoading,
    isAuthorizing: state.applicationParams.isAuthorizing,
    isConfiguring: state.applicationParams.isConfiguring,
    isTeamUpConfiguring: state.applicationParams.isTeamUpConfiguring,
})


export default connect(
    mapStateToProps,
    null
)(App)