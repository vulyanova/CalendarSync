import { takeLatest , call, put } from "redux-saga/effects";
import axios from "axios";

function getCalendars(user) {
    return axios.get('https://localhost:5001/api/calendars/'+ user);
}

function getTeamUpCalendars(user) {
    return axios.get('https://localhost:5001/api/teamUpCalendars/'+ user);
}

function getTimers() {
    return axios.get('https://localhost:5001/api/configurations/timers');
}

export function* getConfigsDataSaga() {
    yield takeLatest('GET_CONFIGS_DATA', workerSaga);
}

function* workerSaga(action) {
    const user = action.user;

    var calendarsResponse = yield call(getCalendars, user);
    const teamUpResponse = yield call(getTeamUpCalendars, user);
    const timersResponse = yield call(getTimers);

    const calendars = calendarsResponse.data;
    yield put({ type: "CHANGE_CALENDARS", calendars });

    const teamUpCalendars = teamUpResponse.data;
    yield put({ type: "CHANGE_TEAM_UP_CALENDARS", teamUpCalendars });

    
    const timers = timersResponse.data;
    const timer = timers[0].ms;

    yield put({ type: "CHANGE_TIMER",  timer});
    yield put({ type: "CHANGE_TIMERS", timers });

    yield put({ type: "STOP_AUTHORIZING"})
    yield put({ type: "SUCCESS_LOADING"})
}