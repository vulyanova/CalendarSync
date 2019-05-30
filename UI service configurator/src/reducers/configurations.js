const initialState = {
    user: '',
    timer: 0,
    showSummary: false,
    calendar: "primary",
    teamUpCalendar: "",
    calendars: [{"name": "primary", "id": "primary"}],
    teamUpCalendars: [{"name": "primary", "id": 0}]
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
        case 'CHANGE_TEAM_UP_CALENDARS':
                return {
                    ...state,
                    teamUpCalendars: action.teamUpCalendars,
                    teamUpCalendar: action.teamUpCalendars[0].id
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
        case 'CHANGE_TEAM_UP_CALENDAR':
                return {
                    ...state,
                    teamUpCalendar: action.teamUpCalendar
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