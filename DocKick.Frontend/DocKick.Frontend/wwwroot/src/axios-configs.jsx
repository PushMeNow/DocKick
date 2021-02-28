import axios from "axios";
import { useContext } from "react";
import { AuthContext } from "./context/auth-context";
import { LoaderContext } from "./context/loader-context";

export const AxiosConfig = () => {
    let counter = 0;
    
    const authContext = useContext(AuthContext);
    const loaderContext = useContext(LoaderContext);
    const authHeader = authContext.getAuthorizationHeader();
    const onResponse = responseConfig => {
        if(counter > 0){
            counter--;
        }

        if (counter === 0) {
            loaderContext.hideLoader();
        }

        return responseConfig;
    };
    

    if (!!authHeader) {
        axios.defaults.headers.common['Authorization'] = authHeader;
    }

    axios.interceptors.request.use(requestConfig => {
        counter++;
        
        if (counter > 0){
            loaderContext.showLoader();
        }

        return requestConfig;
    });
    
    axios.interceptors.response.use(onResponse, onResponse);

    return <></>;
}