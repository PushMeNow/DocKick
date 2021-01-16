import axios from "axios";
import { useContext } from "react";
import { AuthContext } from "./context/auth-context";

export const AxiosConfig = () => {
    const context = useContext(AuthContext)
    const authHeader = context.getAuthorizationHeader();

    if (!!authHeader) {
        axios.defaults.headers.common['Authorization'] = authHeader;
    }
    
    return <></>;
}