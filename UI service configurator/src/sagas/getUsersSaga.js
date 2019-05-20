import { takeLatest, call, put } from "redux-saga/effects";
import axios from "axios";

export function* getUsersSaga() {
    yield takeLatest('GET_USERS', workerSaga);
}

function getUsers() {
    return axios.get('https://localhost:5001/api/authorize/');
}

function* workerSaga() {
    const response = yield call(getUsers);
    const users = response.data;
    
    yield put({ type: "CHANGE_USERS", users });
    yield put({ type: "USERS_FETCHED" });
    yield put({ type: "SUCCESS_LOADING" })
}