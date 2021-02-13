import axios from "axios";
import { useContext } from "react";
import { AuthContext } from "./context/auth-context";

export const AxiosConfig = () => {
    const authContext = useContext(AuthContext)
    const authHeader = authContext.getAuthorizationHeader();

    if (!!authHeader) {
        axios.defaults.headers.common['Authorization'] = authHeader;
    }

    axios.interceptors.request.use(requestConfig => {
        if (requestConfig.before) {
            requestConfig.before();
        }

        return requestConfig;
    });

    return <></>;
}