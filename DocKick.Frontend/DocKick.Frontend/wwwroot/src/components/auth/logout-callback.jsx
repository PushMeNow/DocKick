import React, { useContext } from "react";
import { AuthContext } from "../../context/auth-context";

export const LogoutCallback = () => {
    const { signoutRedirectCallback } = useContext(AuthContext);

    signoutRedirectCallback();

    return <div>Please wait one sec...</div>
}