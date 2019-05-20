import { connect } from 'react-redux';
import Timer from '../components/Timer';
import { changeTimer } from '../actions'

const mapDispatchToProps = dispatch => {
    return {
        changeTimer: (timer) => dispatch(changeTimer(timer)),
    }
}

const mapStateToProps = (state) => ({
    timers: state.timers
})

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(Timer)