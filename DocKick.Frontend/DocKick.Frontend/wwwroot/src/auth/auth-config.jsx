import globalConfig from "../config";

const authConfig = {
    google: {
        clientId: '734351612309-0bl0o4vlsmfooue95ellut0fc833scmt.apps.googleusercontent.com',
        loginEndpoint: `${globalConfig.authServerUrl}/account/google-login` 
    }
};

export default authConfig;