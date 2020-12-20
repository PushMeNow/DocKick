import React, { Component } from "react";
import { GoogleLogout } from "react-google-login";
import authConfig from "../auth-config";
import { logout } from "../authActions";
import { connect } from "react-redux"

class DocKickGoogleLogout extends Component {
    onSuccess = () => {
        this.props.logout();
    };
    
    render() {
        return !this.props.auth.isAuthenticated ?
            (
                <div>User is not authenticated</div>
            )
            : (
                <div>
                    <GoogleLogout clientId={ authConfig.google.clientId }
                                  buttonText="Logout"
                                  onLogoutSuccess={ this.onSuccess } />
                </div>
            )
    }
}

const mapStateToProps = (state) => {
    return {
        auth: state.authReducer
    };
};

const mapDispatchToProps = (dispatch) => {
    return {
        logout: () => {
            dispatch(logout());
        }
    }
};

export default connect(mapStateToProps, mapDispatchToProps)(DocKickGoogleLogout);