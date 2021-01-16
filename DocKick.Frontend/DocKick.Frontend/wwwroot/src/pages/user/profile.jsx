import React from "react";
import axios from "axios";
import { combineIdentityServerUrl } from "../../url-helper";
import { Button } from "react-bootstrap";

export const Profile = () => {
    const onClick = () => {
        axios(combineIdentityServerUrl('account/profile'), { 
            method: 'get' 
        })
            .then(response => {
                debugger;
            });
    }

    return <>
        <Button onClick={ onClick }
                variant="primary">Profile</Button>
    </>;
}