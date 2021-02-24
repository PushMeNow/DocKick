import axios from "axios";
import { useContext } from "react";
import { AuthContext } from "./context/auth-context";
import { LoaderContext } from "./context/loader-context";

export const AxiosConfig = () => {
    const authContext = useContext(AuthContext);
    const loaderContext = useContext(LoaderContext);
    const authHeader = authContext.getAuthorizationHeader();
    let counter = 0;

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
    
    axios.interceptors.response.use(responseConfig => {
        if(counter > 0){
            counter--;
        }
        
        if (counter === 0) {
            loaderContext.hideLoader();
        }
        
        return responseConfig;
    });

    return <></>;
}