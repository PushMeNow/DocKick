import React, { Component } from "react";
import { AuthProvider } from "../../context/auth-context";
import { LoginCallback } from "../../components/auth/login-callback";

export class LoginCallbackPage extends Component {
    render() {
        return <AuthProvider>
            <LoginCallback />
        </AuthProvider>
    }
}