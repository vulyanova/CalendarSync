import { takeLatest, call, put } from "redux-saga/effects";
import axios from "axios";

export function* getTimersSaga() {
    yield takeLatest('GET_TIMERS', workerSaga);
}

function getTimers() {
    return axios.get('https://localhost:5001/api/configurations/timers');
}

function* workerSaga() {
    const response = yield call(getTimers);
    const timers = response.data;
    const timer = timers[0].ms;

    yield put({ type: "CHANGE_TIMER",  timer});
    yield put({ type: "CHANGE_TIMERS", timers });
}