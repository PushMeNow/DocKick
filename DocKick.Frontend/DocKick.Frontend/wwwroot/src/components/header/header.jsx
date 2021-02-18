import { Nav, Navbar } from "react-bootstrap";
import React, { useContext } from "react";
import { Link } from "react-router-dom";
import { AuthContext } from "../../context/auth-context";
import { LogoutButton } from "../auth/logout-button";
import { LoginButton } from "../auth/login-button";
import { UserMenu } from "./user-menu";
import NavbarCollapse from "react-bootstrap/NavbarCollapse";

const Header = () => {
    const { isAuthenticated } = useContext(AuthContext);

    let loginBtn = !isAuthenticated()
        ? <LoginButton />
        : <LogoutButton />;

    return (
        <Navbar bg="light"
                expand="lg">
            <Navbar.Brand>
                <Link className="navbar-brand"
                      to="/">Doc Kick</Link>
            </Navbar.Brand>
            <Navbar.Toggle aria-controls="dockick-navbar" />
            <NavbarCollapse id="dockick-navbar">
                <Nav className="mr-auto">
                    <Link to="/"
                          className="nav-link">Home</Link>
                    <UserMenu />
                </Nav>
                <Nav>
                    { loginBtn }
                </Nav>
            </NavbarCollapse>
        </Navbar>
    )
}

export default Header;