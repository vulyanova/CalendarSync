export const changeUser = (user) => ({
    type: 'CHANGE_USER',
    user
})
export const changeTimer = (timer) => ({
    type: 'CHANGE_TIMER',
    timer 
})

export const changeClientId = (clientId) => ({
    type: 'CHANGE_CLIENT_ID',
    clientId
})

export const changeClientSecret = (clientSecret) => ({
    type: 'CHANGE_CLIENT_SECRET',
    clientSecret
})

export const changeCalendar = (calendar) => ({
    type: 'CHANGE_CALENDAR',
    calendar
})

export const changeId = (id) => ({
    type: 'CHANGE_ID',
    id
})

export const changeSummary = (showSummary) => ({
    type: 'CHANGE_SUMMARY',
    showSummary
})


export const startAuthorizing = () => ({
    type: 'START_AUTHORIZING'
})

export const stopAuthorizing = () => ({
    type: 'STOP_AUTHORIZING'
})

export const stopConfiguring = () => ({
    type: 'STOP_CONFIGURING'
})

export const startConfigiring = () => ({
    type: 'START_CONFIGURING',
})

export const startService = () => ({
    type: "START_SERVICE"
})

export const stopService = (id) => ({
    type: "STOP_SERVICE",
    id
})

export const authorize = (authorizationParams) => ({
    type: "AUTHORIZE",
    authorizationParams
})

export const getCalendars = (user) => ({
    type: "GET_CALENDARS",
    user
})

export const sendConfigurations = (configurations) => ({
    type: "ADD_CONFIGS",
    configurations
})

export const getUsers = () => ({
    type: "GET_USERS"
})
