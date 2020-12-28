import { createStore, combineReducers, applyMiddleware } from "redux";
import { createLogger } from "redux-logger/src";
import thunk from "redux-thunk";
import authReducer, { authState } from "./reducers/authReducer";
import { composeWithDevTools } from 'redux-devtools-extension';

const rootReducer = combineReducers({ auth: authReducer });
const initialState = {
    auth: authState
};
const logger = createLogger();

export default createStore(rootReducer,
                           initialState,
                           composeWithDevTools(applyMiddleware(logger, thunk)));