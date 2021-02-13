import React from "react";
import Header from "./components/header/header";
import { BrowserRouter, Route, Switch } from 'react-router-dom';
import PrivateRoute from "./components/private-route";
import PrivateLayout from "./layouts/router-layouts/private-layout";
import PublicLayout from "./layouts/router-layouts/public-layout";
import { Container } from "react-bootstrap";
import 'bootstrap/dist/css/bootstrap.min.css'
import { AuthProvider } from "./context/auth-context";
import { AxiosConfig } from "./axios-configs";
import 'react-toastify/dist/ReactToastify.css';
import { ToastContainer } from "react-toastify";
import { LoaderProvider } from "./context/loader-context";

const App = React.memo(() => (
    <>
        <BrowserRouter>
            <AuthProvider>
                <LoaderProvider>
                    <AxiosConfig />
                    <ToastContainer autoClose={ 2000 }
                                    position={ "bottom-center" } />
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
                </LoaderProvider>
            </AuthProvider>
        </BrowserRouter>
    </>
));

export default App;