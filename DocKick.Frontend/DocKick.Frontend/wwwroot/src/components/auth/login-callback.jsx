import React, { useContext } from "react";
import { AuthContext } from "../../context/auth-context"
import { LoaderContext } from "../../context/loader-context";
import MainPage from "../../pages/main/main-page";

export const LoginCallback = () => {
    const { signinRedirectCallback } = useContext(AuthContext);
    const { showLoader } = useContext(LoaderContext);

    showLoader();

    signinRedirectCallback();

    return <MainPage />
}