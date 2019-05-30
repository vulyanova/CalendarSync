import { connect } from 'react-redux';
import TeamUpCalendar from '../components/TeamUpCalendar';
import { changeTeamUpCalendar } from '../actions'

const mapDispatchToProps = dispatch => {
    return {
        changeTeamUpCalendar: (teamUpCalendar) => dispatch(changeTeamUpCalendar(teamUpCalendar)),
    }
}

const mapStateToProps = (state)  => {
    return {
        teamUpCalendars: state.configurations.teamUpCalendars,
        teamUpCalendar: state.configurations.teamUpCalendar
    }
}

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(TeamUpCalendar)