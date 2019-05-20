import { connect } from 'react-redux';
import Variants from '../components/Variants';
import {
    startViewing,
    loadConfigs
} from '../actions'


const mapStateToProps = (state) => ({
    variants: state.applicationParams.variants,
    isChoosing: state.applicationParams.isChoosing
})

const mapDispatchToProps = dispatch => {
    return {      
        startViewing: () => dispatch(startViewing()),
        loadConfigs: (user) => dispatch(loadConfigs(user))
    }
}



export default connect(
    mapStateToProps,
    mapDispatchToProps
)(Variants)