import React, { useContext } from "react";
import { AuthContext } from "../../context/auth-context";
import { NavDropdown } from "react-bootstrap";
import { DropdownLink } from "./dropdown-link";

export const UserMenu = () => {
    const { isAuthenticated } = useContext(AuthContext);

    if (!isAuthenticated()) {
        return null;
    }

    return (
        <NavDropdown title="User Profile">
            <DropdownLink to="/profile">Profile</DropdownLink>
            <DropdownLink to="/categories">Categories</DropdownLink>
            <DropdownLink to="/category-tree">Category Tree</DropdownLink>
            <DropdownLink to="/files">Files</DropdownLink>
        </NavDropdown>
    )
}