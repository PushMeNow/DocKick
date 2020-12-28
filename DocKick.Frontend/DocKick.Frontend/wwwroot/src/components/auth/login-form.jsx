import React, { Component, useState } from "react";
import { Button, Col, Form, FormControl, FormGroup, FormLabel, Row } from "react-bootstrap";
import authConfig from "../../auth-config";
import { connect, useDispatch } from "react-redux";
import DocKickGoogleLogin from "../auth/google/doc-kick-google-login";
import axios from "axios";
import { loginThunk } from "../../actions/authThunks";

const LoginForm = () => {

    const [{email, password}, setValues] = useState({
                                             email: '',
                                             password: ''
                                         });
    const dispatch = useDispatch();

    const onChange = ({ target: { name, value } }) => {
        setValues((prevValues) => ({
            ...prevValues,
            [name]: value,
        }));
    }

    const onClick = async () => {
        if (!email || !password) {
            alert('Incorrect data');

            return;
        }

        dispatch(loginThunk({email, password}));
    }

    return (
        <Row className="mt-2 justify-content-center">
            <Col sm={ 6 }>
                <Form.Group>
                    <FormLabel>Email</FormLabel>
                    <FormControl type="email"
                                 name="email"
                                 placeholder="Enter email address"
                                 onChange={ onChange }
                                 value={ email } />
                </Form.Group>
                <FormGroup>
                    <FormLabel>Password</FormLabel>
                    <FormControl type="password"
                                 name="password"
                                 placeholder="Enter password"
                                 onChange={ onChange }
                                 value={ password } />
                </FormGroup>
                <Row>
                    <Col>
                        <Button type="button"
                                onClick={ onClick }>Login</Button>
                        <DocKickGoogleLogin />
                    </Col>
                </Row>
            </Col>
        </Row>
    )
}

export default connect()(LoginForm);