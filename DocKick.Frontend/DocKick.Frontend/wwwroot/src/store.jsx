﻿import { createStore, combineReducers, applyMiddleware } from "redux";
import { createLogger } from "redux-logger/src";
import thunk from "redux-thunk";
import authReducer from "./auth/authReducer";


export default createStore(combineReducers({
                                               authReducer
                                           }),
                           {},
                           applyMiddleware(createLogger(), thunk)
);