import { Navbar, Nav } from "react-bootstrap";
import React, { useContext } from "react";
import { Link } from "react-router-dom";
import { AuthContext } from "../../context/auth-context";
import { LogoutButton } from "../auth/logout-button";
import { LoginButton } from "../auth/login-button";

const Header = () => {
    const { isAuthenticated } = useContext(AuthContext);

    let button = !isAuthenticated()
        ? <LoginButton />
        : <LogoutButton />;

    return (
        <Navbar bg="light"
                expand="lg">
            <Navbar.Brand>
                <Link className="navbar-brand"
                      to="/">Doc Kick</Link>
            </Navbar.Brand>
            <Nav className="mr-auto">
                <Link to="/"
                      className="nav-link">Home</Link>
                { isAuthenticated() && <Link to="/profile"
                                             className="nav-link">Profile</Link>
                }
            </Nav>
            <Nav>
                { button }
            </Nav>
        </Navbar>
    )
}

export default Header;