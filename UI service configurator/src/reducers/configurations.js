const initialState = {
    user: '',
    timer: 0,
    showSummary: false,
    calendar: "primary",
    calendars: [{"name": "primary", "id": "primary"}]
}; 

export const configurations = (state = initialState, action) => {
    switch (action.type) {
        case 'CHANGE_USER':
            return {
                ...state,
                user: action.user
            }
        case 'CHANGE_CALENDARS':
            return {
                ...state,
                calendars: action.calendars,
                calendar: action.calendars[0].id
            }
        case 'CHANGE_TIMER':
            return {
                ...state,
                timer: action.timer
            }
        case 'CHANGE_CALENDAR':
            return {
                ...state,
                calendar: action.calendar
            }
        case 'CHANGE_SUMMARY':
            return {
                ...state,
                showSummary: action.showSummary
            }
        default:
            return state;
    }
}   