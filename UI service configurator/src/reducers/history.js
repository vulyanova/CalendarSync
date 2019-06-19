const initialState = {
    calendars: []
};

export const history = (state = initialState, action) => {
    switch (action.type) {
        case 'CHANGE_HISTORY_CALENDARS':
            return {
                ...state,
                calendars: action.calendars
            }
        case 'ADD_HISTORY_CALENDARS':
            return {
                ...state,
                calendars: [...state.calendars, ...action.calendars]
            }
        default:
            return state;
    }
}   