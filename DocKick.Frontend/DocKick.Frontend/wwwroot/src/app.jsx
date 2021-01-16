import React from "react";
import Header from "./components/header/header";
import { Switch, Route, BrowserRouter } from 'react-router-dom';
import PrivateRoute from "./components/private-route";
import PrivateLayout from "./layouts/router-layouts/private-layout";
import PublicLayout from "./layouts/router-layouts/public-layout";
import { Container } from "react-bootstrap";
import 'bootstrap/dist/css/bootstrap.min.css'
import { AuthProvider } from "./context/auth-context";
import { AxiosConfig } from "./axios-configs";

const App = React.memo(() => (
    <>
        <BrowserRouter>
            <AuthProvider>
                <AxiosConfig />
                <Header />
                <Container>
                    <Switch>
                        <Route path="/login-callback"
                               component={ PublicLayout } />
                        <Route path="/logout-callback"
                               component={ PublicLayout } />
                        <Route path="/profile"
                               component={ PublicLayout } />
                        <PrivateRoute path="/"
                                      component={ PrivateLayout } />
                    </Switch>
                </Container>
            </AuthProvider>
        </BrowserRouter>
    </>
));

export default App;