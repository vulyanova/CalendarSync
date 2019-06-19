import { connect } from 'react-redux';
import History from '../components/History';
import {
    changeHistoryState,
    addHistoryState
} from '../actions'

const mapDispatchToProps = dispatch => {
    return {
        changeHistoryState: () => dispatch(changeHistoryState()),
        addHistoryState: (size, page) => dispatch(addHistoryState(size, page))
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