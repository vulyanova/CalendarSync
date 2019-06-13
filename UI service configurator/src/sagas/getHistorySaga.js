import { takeLatest, call, put } from "redux-saga/effects";
import axios from "axios";

export function* getHistorySaga() {
    yield takeLatest('GET_HISTORY', workerSaga);
}

function getHistory(user) {
    return axios.get('https://localhost:5001/api/history/');
}

function* workerSaga(action) {
    const response = yield call(getHistory, action.user);
    const calendars = response.data;

    yield put({ type: "CHANGE_HISTORY_CALENDARS", calendars })
    
    yield put({ type: "STOP_AUTHORIZING" })
    yield put({ type: "SHOW_HISTORY" })
    yield put({ type: "SUCCESS_LOADING" })
}