import React from "react";
import { useContext } from "react";
import { AuthContext } from "../../context/auth-context";
import { Button } from "react-bootstrap";

export const LoginButton = () => {
    const { signinRedirect } = useContext(AuthContext);

    const onClick = () => {
        signinRedirect();
    }

    return <Button variant="primary"
                   onClick={ onClick }>Login</Button>
}