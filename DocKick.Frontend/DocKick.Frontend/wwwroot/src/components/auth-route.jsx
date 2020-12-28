import React from "react";
import { connect } from "react-redux";
import { Redirect, Route } from "react-router-dom";

const AuthRoute = props => {
    const { isAuthenticated, type } = props;
    if (type === "guestOnly" && isAuthenticated) {
        return <Redirect to="/" />;
    } else if (type === "private" && !isAuthenticated) {
        return <Redirect to="/" />;
    }

    return <Route { ...props } />;
};

export default connect(({ auth }) => {
    return {
        isAuthenticated: auth.isAuthenticated
    };
})(AuthRoute);