import React, { Component } from 'react';
import GoogleLogin from "react-google-login";
import authConfig from "../../../auth-config";
import { connect } from "react-redux";
import { login } from "../../../reducers/authActions";
import { Button } from "react-bootstrap";

class DocKickGoogleLogin extends Component {
    responseGoogle = (response) => {
        const tokenBlob = new Blob([JSON.stringify({ tokenId: response.tokenId }, null, 2)], { type: 'application/json' });
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
                     const token = user.token;
                     this.props.login(token);
                 });
            })
    };

    render() {
        return !!this.props.auth.isAuthenticated ?
            (
                <div>Authenticated</div>
            )
            : (
                <div className="mt-2">
                    <GoogleLogin
                        clientId={ authConfig.google.clientId }
                        render={ renderProps => (
                            <Button variant="outline-secondary"
                                    onClick={ renderProps.onClick }
                                    disabled={ renderProps.disabled }>Google</Button>)
                        }
                        onSuccess={ this.responseGoogle }
                        onFailure={ this.responseGoogle }
                        cookiePolicy={ 'single_host_origin' } />
                </div>
            );
    }
}

const mapStateToProps = (state) => {
    return {
        auth: state.authReducer
    };
};

const mapDispatchToProps = (dispatch) => {
    return {
        login: (token) => {
            dispatch(login(token));
        }
    }
};

export default connect(mapStateToProps, mapDispatchToProps)(DocKickGoogleLogin);