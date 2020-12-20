import React from "react";
import Header from "./components/header/header";
import { Switch, Route, BrowserRouter } from 'react-router-dom';
import PrivateRoute from "./components/private-route";
import PrivateLayout from "./layouts/router-layouts/private-layout";
import PublicLayout from "./layouts/router-layouts/public-layout";
import { Container } from "react-bootstrap";

const App = React.memo(() => (
    <>
        <BrowserRouter>
            <Header />
            <Container>
                <Switch>
                    <Route path="/login"
                           component={ PublicLayout } />
                    <PrivateRoute path="/"
                                  component={ PrivateLayout } />
                </Switch>
            </Container>
        </BrowserRouter>
    </>
));

export default App;