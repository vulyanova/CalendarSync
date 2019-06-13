import { connect } from 'react-redux';
import App from '../components/App'

const mapStateToProps = (state) => ({   
    isLoading: state.applicationParams.isLoading,
    isAuthorizing: state.applicationParams.isAuthorizing,
    isConfiguring: state.applicationParams.isConfiguring,
    isHistoryShown: state.applicationParams.isHistoryShown,
})


export default connect(
    mapStateToProps,
    null
)(App)