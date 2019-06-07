import { connect } from 'react-redux';
import History from '../components/History';
import {
    changeHistoryState
} from '../actions'

const mapDispatchToProps = dispatch => {
    return {
        changeHistoryState: () => dispatch(changeHistoryState())
    }
}


const mapStateToProps = (state) => ({
    calendars: state.history.calendars,
    currentState: state.history.currentState
})


export default connect(
    mapStateToProps,
    mapDispatchToProps
)(History)