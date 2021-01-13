import React, { useContext } from "react";
import { AuthContext } from "../../context/auth-context";
import { Button } from "react-bootstrap";

export const LogoutButton = () => {
    const { logout, isAuthenticated } = useContext(AuthContext);
    
    const onClick = () => {
        logout();
    };

    return !isAuthenticated() ?
        (
            <div>User is not authenticated</div>
        )
        : (
            <div>
                <Button variant="outline-secondary"
                        onClick={ onClick }>Logout</Button>
            </div>
        )
}