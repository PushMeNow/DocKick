import React, { useContext, useState } from "react";
import axios from "axios";
import { AuthContext } from "./context/auth-context";
import { Loader } from "./components/loader/loader";

export const AxiosConfig = () => {
    let counter = 0;

    const [loading, setLoading] = useState(false);
    const { getAuthorizationHeader } = useContext(AuthContext);
    const authHeader = getAuthorizationHeader();
    const toggleLoader = () => {
        setLoading(() => counter !== 0);
    };

    const onResponse = responseConfig => {
        if (counter > 0) {
            counter--;
        }

        toggleLoader();

        return responseConfig;
    };

    if (!!authHeader) {
        axios.defaults.headers.common['Authorization'] = authHeader;
    }

    axios.interceptors.request.use(requestConfig => {
        counter++;

        toggleLoader();

        return requestConfig;
    });

    axios.interceptors.response.use(onResponse, onResponse);

    return <Loader showLoader={ loading } />;
}