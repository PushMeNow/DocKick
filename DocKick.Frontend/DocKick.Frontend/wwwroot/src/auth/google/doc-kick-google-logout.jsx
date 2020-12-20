import React, {Component} from "react";
import {GoogleLogout} from "react-google-login";
import authConfig from "../auth-config";

const onSuccess = () => {
    alert('Logout made successfully.');
}

class DocKickGoogleLogout extends Component {
    render() {
        return (
            <div>
                <GoogleLogout clientId={authConfig.google.clientId} buttonText='Logout' onLogoutSuccess={onSuccess}/>
            </div>
        )
    }
}

export default DocKickGoogleLogout;