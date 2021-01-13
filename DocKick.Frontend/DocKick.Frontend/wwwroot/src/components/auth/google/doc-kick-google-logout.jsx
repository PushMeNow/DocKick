import React, { Component, useContext } from "react";
import { GoogleLogout } from "react-google-login";

import { Button } from "react-bootstrap";
import { authConfig } from "../../../auth-config";
import { AuthContext } from "../../../context/auth-context";

const DocKickGoogleLogout = () => {
    const context = useContext(AuthContext);

    const onSuccess = () => {
        context.logout();
    };

    return !context.isAuthenticated() ?
        (
            <div>User is not authenticated</div>
        )
        : (
            <div>
                <GoogleLogout clientId={ authConfig.google.clientId }
                              render={ renderProps => (
                                  <Button variant="outline-secondary"
                                          onClick={ renderProps.onClick }>Logout</Button>)
                              }
                              onLogoutSuccess={ onSuccess } />
            </div>
        )

}

export default DocKickGoogleLogout;