import { takeLatest, call, put } from "redux-saga/effects";
import axios from "axios";

export function* addConfigsSaga() {
    yield takeLatest('ADD_CONFIGS', workerSaga);
}

function postConfigs(configurations) {
    return axios.post('https://localhost:5001/api/configurations/', {
        user: configurations.user,
        timer: configurations.timer,
        calendar: configurations.calendar,
        showSummary: configurations.showSummary
    });
}

function* workerSaga(action) {

    yield call(postConfigs, action.configurations);

    yield put({ type: "STOP_CONFIGURING" })
    yield put({ type: "SUCCESS_LOADING" })


}