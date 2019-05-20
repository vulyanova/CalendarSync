import { connect } from 'react-redux';
import Summary from '../components/ShowSummary';
import { changeSummary } from '../actions'

const mapDispatchToProps = dispatch => {
    return {
        changeSummary: (summary) => dispatch(changeSummary(summary)),
    }
}

export default connect(
    null,
    mapDispatchToProps
)(Summary)