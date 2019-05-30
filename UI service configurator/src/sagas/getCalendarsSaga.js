import { takeLatest, call, put } from "redux-saga/effects";
import axios from "axios";

export function* getCalendarsSaga() {
    yield takeLatest('GET_CALENDARS', workerSaga);
}

function getCalendars(user) {
    return axios.get('https://localhost:5001/api/calendars/'+ user);
}

function* workerSaga(action) {
    const response = yield call(getCalendars, action.user);
    const calendars = response.data;
    const user = action.user;
    
    yield put ({ type: "GET_TIMERS" });
    yield put ({ type: "GET_TEAM_UP_CALENDARS", user})
    yield put({ type: "CHANGE_CALENDARS", calendars });
    yield put({ type: "SUCCESS_LOADING" })
    yield put({ type: "STOP_AUTHORIZING" })
}