import React, { useContext } from "react";
import { AuthContext } from "../../context/auth-context";
import { LoginButton } from "./login-button";
import { LogoutButton } from "./logout-button";

export const LoginLogoutButton = () => {
    const { isAuthenticated } = useContext(AuthContext);

    return !isAuthenticated()
        ? <LoginButton />
        : <LogoutButton />;
}