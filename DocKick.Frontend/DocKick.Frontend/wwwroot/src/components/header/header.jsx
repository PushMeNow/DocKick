import { Navbar, Nav, NavLink } from "react-bootstrap";
import React from "react";
import { Component } from "react";
import DocKickGoogleLogout from "../auth/google/doc-kick-google-logout";
import { connect } from "react-redux";
import { Link } from "react-router-dom";

class Header extends Component {
    render() {
        let button;

        if (!this.props.auth.isAuthenticated) {
            button = <Link to="/login">Login</Link>;
        } else {
            button = <DocKickGoogleLogout />;
        }

        return (
            <Navbar bg="light"
                    expand="lg">
                <Navbar.Brand>
                    <Link className="navbar-brand"
                          to="/">Doc Kick</Link>
                </Navbar.Brand>
                <Nav className="mr-auto">
                    <Link to="/" className="nav-link">Home</Link>
                </Nav>
                <Nav>
                    { button }
                </Nav>
            </Navbar>
        )
    }
}

export default connect((state) => {
    return {
        auth: state.authReducer
    }
})(Header);