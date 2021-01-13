import React, { useContext } from "react";
import { AuthContext } from "../../context/auth-context"

export const LoginCallback = () => {
    const context = useContext(AuthContext)

    context.signinRedirectCallback();

    return <span>loading</span>
}