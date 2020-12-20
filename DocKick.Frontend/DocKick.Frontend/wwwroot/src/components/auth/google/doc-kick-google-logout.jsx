import React, { Component } from "react";
import { GoogleLogout } from "react-google-login";
import authConfig from "../../../auth-config";
import { logout } from "../../../reducers/authActions";
import { connect } from "react-redux"
import { Button } from "react-bootstrap";

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
                                  render={ renderProps => (
                                      <Button variant="outline-secondary"
                                              onClick={ renderProps.onClick }
                                              disabled={ renderProps.disabled }>Logout</Button>)
                                  }
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