import axios from "axios";
import { useContext } from "react";
import { AuthContext } from "./context/auth-context";
import { LoaderContext } from "./context/loader-context";

export const AxiosConfig = () => {
    let counter = 0;

    const { getAuthorizationHeader } = useContext(AuthContext);
    const { showLoader, hideLoader } = useContext(LoaderContext);
    const authHeader = getAuthorizationHeader();
    const onResponse = responseConfig => {
        if (counter > 0) {
            counter--;
        }

        if (counter === 0) {
            hideLoader();
        }

        return responseConfig;
    };


    if (!!authHeader) {
        axios.defaults.headers.common['Authorization'] = authHeader;
    }

    axios.interceptors.request.use(requestConfig => {
        counter++;

        if (counter > 0) {
            showLoader();
        }

        return requestConfig;
    });

    axios.interceptors.response.use(onResponse, onResponse);

    return <></>;
}