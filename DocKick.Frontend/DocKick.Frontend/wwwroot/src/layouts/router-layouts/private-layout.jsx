import React from "react";
import { Switch, Route } from 'react-router-dom';
import Main from "../../pages/main/main-page";

const PrivateLayout = () => {
    return (
        <Switch>
            <Route exact
                   path="/"
                   component={ Main } />
        </Switch>
    );
};

export default PrivateLayout;