import React, { useContext } from "react";
import { AuthContext } from "../../context/auth-context";

export const LogoutCallback = () => {
    const context = useContext(AuthContext);

    context.signoutRedirectCallback();
    
    return <div>loading...</div>
}