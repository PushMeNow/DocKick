export const LOGIN_SUCCESS = 'LOGIN';
export const LOGOUT = 'LOGOUT';
export const LOGIN_START = 'LOGIN_START';
export const LOGIN_FAILED = 'LOGIN_FAILED';
export const LOGOUT_START = 'LOGOUT_START'; 

export const loginSuccess = (user) => {
    return {
        type: LOGIN_SUCCESS,
        payload: user
    };
}

export const loginFailed = (message) => {
    return {
        type: LOGIN_FAILED,
        error: message
    }
}

export function logout() {
    return {
        type: LOGOUT
    }
}