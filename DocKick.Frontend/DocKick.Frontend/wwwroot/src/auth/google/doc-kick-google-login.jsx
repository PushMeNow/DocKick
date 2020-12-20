import React, {Component} from 'react';
import GoogleLogin from "react-google-login";
import authConfig from "../auth-config";

const responseGoogle = (response) => {
    const tokenBlob = new Blob([JSON.stringify({tokenId: response.tokenId}, null, 2)], {type: 'application/json'});
    const options = {
        method: 'POST',
        body: tokenBlob,
        mode: 'cors',
        cache: 'default'
    };
    fetch(authConfig.google.loginEndpoint, options)
        .then(r => {
            r.json()
                .then(user => {
                debugger;
                const token = user.token;
                console.log(token);
            });
        })
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