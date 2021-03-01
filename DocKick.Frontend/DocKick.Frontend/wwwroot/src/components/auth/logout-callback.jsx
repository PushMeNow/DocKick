import React, { useContext } from "react";
import { AuthContext } from "../../context/auth-context";
import MainPage from "../../pages/main/main-page";
import { LoaderContext } from "../../context/loader-context";

export const LogoutCallback = () => {
    const { signoutRedirectCallback } = useContext(AuthContext);

    signoutRedirectCallback();

    return <div>Please wait one sec...</div>
}