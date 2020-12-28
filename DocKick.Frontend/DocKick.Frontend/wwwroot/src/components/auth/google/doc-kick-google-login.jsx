import React, { Component } from 'react';
import GoogleLogin from "react-google-login";
import authConfig from "../../../auth-config";
import { connect } from "react-redux";
import { Button } from "react-bootstrap";
import { loginSuccess } from "../../../actions/authActions";
import { googleLoginThunk } from "../../../actions/authThunks";

class DocKickGoogleLogin extends Component {
    responseGoogle = async (response) => {
        this.props.dispatch(googleLoginThunk(response.tokenId))
    };

    render() {
        return !!this.props.auth.isAuthenticated ?
            (
                <div>Authenticated</div>
            )
            : (
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
            );
    }
}

const mapStateToProps = ({auth}) => {
    return {
        auth
    };
};

const mapDispatchToProps = (dispatch) => {
    return {
        login: (token) => {
            dispatch(loginSuccess(token));
        },
        dispatch
    }
};

export default connect(mapStateToProps, mapDispatchToProps)(DocKickGoogleLogin);