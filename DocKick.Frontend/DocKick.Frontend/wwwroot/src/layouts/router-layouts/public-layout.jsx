import React from 'react';
import { Route, Switch } from 'react-router-dom';
import { LoginCallback } from "../../components/auth/login-callback";
import { LogoutCallback } from "../../components/auth/logout-callback";
import Profile from "../../pages/user/profile";

const PublicLayout = () => {
    return (
        <Switch>
            <Route path="/login-callback"
                   component={ LoginCallback } />
            <Route path="/logout-callback"
                   component={ LogoutCallback } />
            <Route path="/profile"
                   component={ Profile } />
        </Switch>
    )
}

export default PublicLayout;