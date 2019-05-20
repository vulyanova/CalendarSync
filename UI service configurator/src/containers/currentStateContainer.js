import { connect } from 'react-redux';
import CurrentState from '../components/CurrentState';

const mapStateToProps = (state) => ({
    user: state.params.user,
    timer: state.params.timer,
    showSummary: state.params.showSummary
})

export default connect(
    mapStateToProps,
    null
)(CurrentState)