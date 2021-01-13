import { Component, createContext } from "react";
import AuthService from "../services/auth-service";
import React from "react";

export const AuthContext = createContext({
                                      signinRedirectCallback: () => ({}),
                                      logout: () => ({}),
                                      signoutRedirectCallback: () => ({}),
                                      isAuthenticated: () => ({}),
                                      signinRedirect: () => ({}),
                                      signinSilentCallback: () => ({}),
                                      createSigninRequest: () => ({})
                                  });

export const AuthConsumer = AuthContext.Consumer;

export class AuthProvider extends Component {
    constructor(props) {
        super(props);
        this.authService = new AuthService();
    }
    
    render() {
        return <AuthContext.Provider value={this.authService}>{this.props.children}</AuthContext.Provider>;
    }
}