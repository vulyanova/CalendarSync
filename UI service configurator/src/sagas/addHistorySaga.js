import { takeLatest, call, put } from "redux-saga/effects";
import axios from "axios";

export function* addHistorySaga() {
    yield takeLatest('ADD_HISTORY_STATE', workerSaga);
}

function addHistory(size, page) {
    return axios.get('https://localhost:5001/api/history/' + size + '/' + page);
}

function* workerSaga(action) {
    const response = yield call(addHistory, action.size, action.page);
    const calendars = response.data;

    yield put({ type: "ADD_HISTORY_CALENDARS", calendars })
}