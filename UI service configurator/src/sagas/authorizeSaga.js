import { takeLatest, call, put } from "redux-saga/effects";
import axios from "axios";

export function* authorizeSaga() {
    yield takeLatest('AUTHORIZE', workerSaga);
}

function authorize(authorizationParams) {
    return axios.post('https://localhost:5001/api/authorize/', authorizationParams);
}

function* workerSaga(action) {
    yield call(authorize, action.authorizationParams);    
    const user = action.authorizationParams.user;

    yield put ({ type: "GET_CALENDARS", user })
}