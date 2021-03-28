import { Nav, Navbar } from "react-bootstrap";
import React from "react";
import { Link } from "react-router-dom";
import { AuthProvider } from "../../context/auth-context";
import { UserMenu } from "./user-menu";
import NavbarCollapse from "react-bootstrap/NavbarCollapse";
import { LoginLogoutButton } from "../auth/login-logout-button";

const Header = () => {
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
                    <AuthProvider>
                        <UserMenu />
                    </AuthProvider>
                </Nav>
                <Nav>
                    <AuthProvider>
                        <LoginLogoutButton />
                    </AuthProvider>
                </Nav>
            </NavbarCollapse>
        </Navbar>
    )
}

export default Header;