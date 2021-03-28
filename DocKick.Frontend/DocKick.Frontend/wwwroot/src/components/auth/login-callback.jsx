import React, { useContext } from "react";
import { AuthContext } from "../../context/auth-context"

export const LoginCallback = () => {
    const { signinRedirectCallback } = useContext(AuthContext);

    signinRedirectCallback();

    return <div>Please wait one sec...</div>
}