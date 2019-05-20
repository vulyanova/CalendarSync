export const timers = (state = [], action) => {
    switch (action.type) {
        case 'CHANGE_TIMERS':
            return action.timers     
        default:
            return state;
    }
}   