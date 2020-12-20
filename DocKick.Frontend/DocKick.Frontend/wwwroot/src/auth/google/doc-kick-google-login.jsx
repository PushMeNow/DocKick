import React, {Component} from 'react';
import GoogleLogin from "react-google-login";
import authConfig from "../auth-config";

const responseGoogle = (response) => {
    console.log(response);
}

class DocKickGoogleLogin extends Component {
    render() {
        return (
            <div>
                <GoogleLogin clientId={authConfig.google.clientId} buttonText='Push'
                             cookiePolicy='single_host_origin' onSuccess={responseGoogle} onFailure={responseGoogle}
                             isSignedIn={true}/>
            </div>
        );
    }
}

export default DocKickGoogleLogin;