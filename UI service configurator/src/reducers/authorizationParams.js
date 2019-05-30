const initialState = {
    clientId: '',
    clientSecret: '',
    user: '',
    accessToken: '',
    refreshToken: '',
    calendarKey: '',
};

export const authorizationParams = (state = initialState, action) => {
    switch (action.type) {
        case 'CHANGE_ID':
            return {
                ...state,
                id: action.id
            }
        case 'CHANGE_CALENDAR_KEY':
                return {
                    ...state,
                    calendarKey: action.calendarKey
                }
        case 'CHANGE_CLIENT_ID':
            return {
                ...state,
                clientId: action.clientId
            }
        case 'CHANGE_CLIENT_SECRET':
            return {
                ...state,
                clientSecret: action.clientSecret
            }
        case 'CHANGE_ACCESS_TOKEN':
            return {
                ...state,
                accessToken: action.accessToken
            }
        case 'CHANGE_REFRESH_TOKEN':
            return {
                ...state,
                refreshToken: action.refreshToken
            }
        case 'CHANGE_USER':
            return {
                ...state,
                user: action.user
            }
        case 'CLEAN_PARAMS':
            return initialState
        default:
            return state;
    }
}   