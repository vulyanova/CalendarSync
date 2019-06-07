const initialState = {
    isAuthorizing: true,
    isLoading: false,
    isConfiguring: true,
    usersFetched: false,
    users: []
};

export const applicationParams = (state = initialState, action) => {
    switch (action.type) {
        case 'SUCCESS_LOADING':
            return {
                ...state,
                isLoading: false
            }
        case 'AUTHORIZE':
            return {
                ...state,
                isLoading: true
            }     
        case 'GET_USERS':
            return {
                ...state,
                isLoading: true
            }
        case 'ADD_CONFIGS':
            return {
                ...state,
                isLoading: true
            }
        case 'STOP_CONFIGURING':
            return {
                ...state,
                isConfiguring: false
            }
        case 'START_CONFIGURING':
            return {
                ...state,
                isConfiguring: true
            }
        case 'START_AUTHORIZING':
            return {
                ...state,
                isAuthorizing: true
            }
        case 'STOP_AUTHORIZING':
            return {
                ...state,
                isAuthorizing: false
            }
        case 'CHANGE_USERS':
            return {
                ...state,
                users: action.users
            }
        case 'USERS_FETCHED':
            return {
                ...state,
                usersFetched: true
            }
        case 'GET_CONFIGS_DATA':
            return {
                ...state,
                isLoading: true
            }
        case 'GET_HISTORY':
            return {
                ...state,
                isLoading: true
            }
        default:
            return state;
    }
}   