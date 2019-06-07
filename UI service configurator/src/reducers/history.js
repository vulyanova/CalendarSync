const initialState = {
    currentState: "present",
    calendars: []
};

export const history = (state = initialState, action) => {
    switch (action.type) {
        case 'CHANGE_HISTORY_CALENDARS':
            return {
                ...state,
                calendars: action.calendars
            }
        case 'CHANGE_HISTORY_STATE':
            return {
                ...state,
                currentState: state.currentState==="present"?"previous":"present"
            }
        default:
            return state;
    }
}   