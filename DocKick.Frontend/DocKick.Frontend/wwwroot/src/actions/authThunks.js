import axios from "axios";
import authConfig from "../auth-config";
import { LOGIN_START, loginFailed, loginSuccess, logout, LOGOUT_START } from "./authActions";

export const loginThunk = ({email, password}) => async (dispatch, getState) => {
    dispatch({ type: LOGIN_START });
    try {
        const { data: user } = await axios.post(authConfig.internal.loginEndpoint, {
            email,
            password
        });

        dispatch(loginSuccess(user));
    } catch (error) {
        dispatch(loginFailed(error.message))
    }
}

export const logoutThunk = () => async (dispatch, getState) => {
    dispatch(logout());
}

export const googleLoginThunk = (tokenId) => async (dispatch, getState) => {
    dispatch({ type: LOGIN_START });
    try {
        const { data: user } = await axios.post(authConfig.google.loginEndpoint, {
            tokenId
        });

        dispatch(loginSuccess(user));
    } catch (error) {
        dispatch(loginFailed(error.message))
    }
}