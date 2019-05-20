import { connect } from 'react-redux';
import Calendar from '../components/Calendar';
import { changeCalendar } from '../actions'

const mapDispatchToProps = dispatch => {
    return {
        changeCalendar: (calendar) => dispatch(changeCalendar(calendar)),
    }
}

const mapStateToProps = (state)  => {
    return {
        calendars: state.configurations.calendars,
        calendar: state.configurations.calendar
    }
}

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(Calendar)