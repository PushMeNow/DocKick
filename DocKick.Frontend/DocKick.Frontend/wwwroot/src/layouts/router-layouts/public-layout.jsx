import React from 'react';
import { Switch } from 'react-router-dom';
import LoginPage from "../../pages/login/login-page";
import AuthRoute from "../../components/auth-route";

const PublicLayout = () => {
    return (
        <Switch>
            <AuthRoute path="/login"
                       type="guestOnly"
                       exact
                       component={ LoginPage } />
        </Switch>
    )
}

export default PublicLayout;