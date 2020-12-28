import { LOGIN_SUCCESS, LOGIN_START, LOGOUT, LOGIN_FAILED } from "../actions/authActions";

export const authState = {
    user: null,
    error: null,
    isAuthenticated: false
};

const authReducer = (state = authState, action) => {
    switch (action.type) {
        case LOGIN_START: {
            return {
                ...state,
                user: null,
                error: null,
                isAuthenticated: false
            };
        }
        case LOGIN_SUCCESS: {
            return {
                ...state,
                user: action.payload,
                isAuthenticated: true,
                error: null
            };
        }
        case LOGIN_FAILED: {
            return {
                ...state,
                user: null,
                isAuthenticated: false,
                error: action.error
            };
        }
        case LOGOUT:{
            return {
                ...state,
                user: null,
                isAuthenticated: false,
                error: null
            };
        }
        default:
            return state;
    }
}

export default authReducer;