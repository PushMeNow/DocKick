import React, { Component } from "react";
import { AuthProvider } from "../../context/auth-context";
import { LogoutCallback } from "../../components/auth/logout-callback";

export class LogoutCallbackPage extends Component {
    render() {
        return <AuthProvider>
            <LogoutCallback />
        </AuthProvider>
    }
}