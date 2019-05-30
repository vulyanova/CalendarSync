import { connect } from 'react-redux';
import CalendarKey from '../components/CalendarKey';
import { 
    changeCalendarKey
 } from '../actions'

const mapDispatchToProps = dispatch => {
    return {
        changeCalendarKey: (calendarKey) => dispatch(changeCalendarKey(calendarKey))
    }
}
export default connect(
    null,
    mapDispatchToProps
)(CalendarKey)