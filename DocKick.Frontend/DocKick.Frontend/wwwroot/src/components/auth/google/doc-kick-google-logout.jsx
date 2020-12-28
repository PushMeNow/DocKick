import React, { Component } from "react";
import { GoogleLogout } from "react-google-login";
import authConfig from "../../../auth-config";

import { useDispatch, useSelector } from "react-redux"
import { Button } from "react-bootstrap";
import { logoutThunk } from "../../../actions/authThunks";

const DocKickGoogleLogout = () => {
    const dispatch = useDispatch();
    const { isAuthenticated } = useSelector(({ auth }) => auth);

    const onSuccess = () => {
        dispatch(logoutThunk());
    };

    return !isAuthenticated ?
        (
            <div>User is not authenticated</div>
        )
        : (
            <div>
                <GoogleLogout clientId={ authConfig.google.clientId }
                              render={ renderProps => (
                                  <Button variant="outline-secondary"
                                          onClick={ renderProps.onClick }
                                          disabled={ renderProps.disabled }>Logout</Button>)
                              }
                              onLogoutSuccess={ onSuccess } />
            </div>
        )

}

export default DocKickGoogleLogout;