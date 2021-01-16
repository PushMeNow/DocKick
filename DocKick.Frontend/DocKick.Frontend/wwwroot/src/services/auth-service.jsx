import { UserManager, WebStorageStateStore, Log } from "oidc-client";
import { identityServerConfig } from "../auth-config";

export default class AuthService {
    UserManager;

    constructor() {
        this.UserManager = new UserManager({
                                               ...identityServerConfig,
                                               userStore: new WebStorageStateStore({ store: window.sessionStorage }),
                                           });
        // Logger
        Log.logger = console;
        Log.level = Log.DEBUG;

        this.UserManager.events.addUserLoaded((user) => {
            if (window.location.href.indexOf("signin-oidc") !== -1) {
                this.navigateToScreen();
            }
        });

        this.UserManager.events.addSilentRenewError((e) => {
            console.log("silent renew error", e.message);
        });

        this.UserManager.events.addAccessTokenExpired(() => {
            console.log("token expired");
            this.signinSilent();
        });
    }

    signinRedirectCallback = () => {
        this.UserManager.signinRedirectCallback().then(() => {
            window.location = "/";
        });
    };


    getUser = async () => {
        const user = await this.UserManager.getUser();

        if (!user) {
            return await this.UserManager.signinRedirectCallback();
        }

        return user;
    };

    parseJwt = (token) => {
        const base64Url = token.split(".")[1];
        const base64 = base64Url.replace("-", "+").replace("_", "/");

        return JSON.parse(window.atob(base64));
    };


    signinRedirect = () => {
        localStorage.setItem("redirectUri", window.location.pathname);
        this.UserManager.signinRedirect({});
    };


    navigateToScreen = () => {
        window.location.replace("/en/dashboard");
    };


    isAuthenticated = () => {
        const oidcStorage = this.getOidcStorage()

        return (!!oidcStorage && !!oidcStorage.access_token)
    };

    signinSilent = () => {
        this.UserManager.signinSilent()
            .then((user) => {
                console.log("signed in", user);
            })
            .catch((err) => {
                console.log(err);
            });
    };
    signinSilentCallback = () => {
        this.UserManager.signinSilentCallback();
    };

    createSigninRequest = () => {
        return this.UserManager.createSigninRequest();
    };

    logout = () => {
        let tokenInfo = this.getOidcStorage();

        this.UserManager.signoutRedirect({
                                             id_token_hint: tokenInfo.id_token
                                         });
        this.UserManager.clearStaleState();
    };

    signoutRedirectCallback = () => {
        this.UserManager.signoutRedirectCallback().then(() => {
            this.clearTokenInfo();
            window.location.replace(process.env.REACT_APP_PUBLIC_URL);
        });

        this.UserManager.clearStaleState();
    };

    getOidcStorage = () => {
        return JSON.parse(sessionStorage.getItem(this.getOidcStorageKey()));
    }

    clearTokenInfo = () => {
        sessionStorage.removeItem(this.getOidcStorageKey());
    }

    getOidcStorageKey = () => {
        return `oidc.user:${ process.env.REACT_APP_AUTH_SERVER }:${ process.env.REACT_APP_IDENTITY_CLIENT_ID }`;
    }

    getAuthorizationHeader = () => {
        const oidcStorage = this.getOidcStorage();

        if (!oidcStorage) {
            return null;
        }

        return `${ oidcStorage.token_type } ${ oidcStorage.id_token }`;
    }
}