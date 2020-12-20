import React, { Component, useState } from "react";
import { Button, Col, Form, FormControl, FormGroup, FormLabel, Row } from "react-bootstrap";
import authConfig from "../../auth-config";
import { login } from "../../reducers/authActions";
import { connect, useDispatch } from "react-redux";
import DocKickGoogleLogin from "../auth/google/doc-kick-google-login";

const LoginForm = () => {
    const [values, setValues] = useState({
                                             email: '',
                                             password: ''
                                         });
    const dispatch = useDispatch();

    const onChange = (name) => (event) => {
        event.persist();
        setValues((prevValues) => ({
            ...prevValues,
            [name]: event.target.value,
        }));
    }

    const onClick = () => {
        if (!values.email || !values.password) {
            alert('Incorrect data');

            return;
        }

        fetch(authConfig.internal.loginEndpoint, {
            body: new Blob([JSON.stringify({
                                               email: values.email,
                                               password: values.password
                                           })], { type: 'application/json' }),
            mode: "cors",
            method: 'POST'
        }).then((response) => {
            response.json().then(user => {
                dispatch(login(user.token));
            });
        }).catch(reason => {
            alert(reason);
        });
    }

    return (
        <Row>
            <Col sm={ 6 }>
                <Form.Group>
                    <FormLabel>Email</FormLabel>
                    <FormControl type="email"
                                 name="email"
                                 placeholder="Enter email address"
                                 onChange={ onChange('email') }
                                 value={ values.email } />
                </Form.Group>
                <FormGroup>
                    <FormLabel>Password</FormLabel>
                    <FormControl type="password"
                                 name="password"
                                 placeholder="Enter password"
                                 onChange={ onChange('password') }
                                 value={ values.password } />
                </FormGroup>
                <Button type="button"
                        onClick={ onClick }>Login</Button>
                <DocKickGoogleLogin />
            </Col>
        </Row>
    )
}

export default connect()(LoginForm);