export const authConfig = {
    google: {
        clientId: '734351612309-0bl0o4vlsmfooue95ellut0fc833scmt.apps.googleusercontent.com',
        loginEndpoint: `${ process.env.REACT_APP_AUTH_SERVER }/account/google-login`
    },
    internal: {
        loginEndpoint: `${ process.env.REACT_APP_AUTH_SERVER }/account/internal-login`
    }
};

export const identityServerConfig = {
    // the URL of our identity server
    authority: process.env.REACT_APP_AUTH_SERVER,
    // this ID maps to the client ID in the identity client configuration
    client_id: process.env.REACT_APP_IDENTITY_CLIENT_ID,
    // URL to redirect to after login
    redirect_uri: `${ process.env.REACT_APP_PUBLIC_URL }/login-callback`,
    response_type: "id_token token",
    // the scopes or resources we would like access to
    scope: "openid profile offline_access api1",
    // URL to redirect to after logout
    post_logout_redirect_uri: `${ process.env.REACT_APP_PUBLIC_URL }/logout-callback`,
}