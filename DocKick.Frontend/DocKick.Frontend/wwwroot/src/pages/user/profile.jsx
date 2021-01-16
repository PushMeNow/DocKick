import React from "react";
import axios from "axios";
import { combineCategorizableUrl, combineIdentityServerUrl } from "../../url-helper";
import { Button } from "react-bootstrap";

export const Profile = () => {
    const onClick = () => {
        axios.get(combineIdentityServerUrl('account/profile'))
            .then(response => {
                debugger;
            });
    }
    
    const onClick2 = () =>{
        axios.get(combineCategorizableUrl('category/api'))
            .then(response => {
                debugger;
            })
    }

    return <>
        <Button onClick={ onClick2 }
                variant="primary">Profile</Button>
    </>;
}