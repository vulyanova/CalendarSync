import { takeLatest, call, put } from "redux-saga/effects";
import axios from "axios";

export function* getTeamUpCalendarsSaga() {
    yield takeLatest('GET_TEAM_UP_CALENDARS', workerSaga);
}

function getTeamUpCalendars(user) {
    return axios.get('https://localhost:5001/api/teamUpCalendars/'+ user);
}

function* workerSaga(action) {
    const response = yield call(getTeamUpCalendars, action.user);
    const teamUpCalendars = response.data;

    yield put({ type: "CHANGE_TEAM_UP_CALENDARS", teamUpCalendars });
    yield put({ type: "SUCCESS_LOADING" })
}