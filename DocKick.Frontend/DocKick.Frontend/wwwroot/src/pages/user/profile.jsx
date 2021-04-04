import React, { Component, createRef } from "react";
import axios from "axios";
import { combineIdentityServerUrl } from "../../url-helper";
import { Button, Form, FormControl } from "react-bootstrap";
import { toastSuccess } from "../../helpers/toast-helpers";

class Profile extends Component {
    constructor(props) {
        super(props);
        this.state = {
            email: '',
            firstName: '',
            lastName: '',
            company: '',
            profession: '',
            country: '',
            city: '',
            phone: ''
        }

        this.emailInput = createRef();
    }

    componentDidMount() {
        axios.get(combineIdentityServerUrl('account/profile'))
             .then(response => {
                 this.setState({ ...response.data });
                 this.emailInput.current.value = response.data.email;
             });
    }

    render() {
        const updateProfile = () => {
            let {
                firstName,
                lastName,
                company,
                profession,
                country,
                city,
                phone
            } = this.state;

            axios.put(combineIdentityServerUrl('account/profile'), {
                firstName,
                lastName,
                company,
                profession,
                country,
                city,
                phone
            }).then(response => {
                this.setState({ ...response.data });

                toastSuccess('Profile was saved successfully.');
            });
        };

        const onChange = (e) => {
            this.setState({ [e.target.name]: e.target.value });
        }

        return (
            <Form>
                <Form.Group>
                    <Form.Label>Email</Form.Label>
                    <FormControl type="text"
                                 ref={ this.emailInput }
                                 disabled />
                </Form.Group>
                <Form.Group>
                    <Form.Label>First Name</Form.Label>
                    <FormControl type="text"
                                 value={ this.state.firstName }
                                 name="firstName"
                                 onChange={ onChange } />
                </Form.Group>
                <Form.Group>
                    <Form.Label>Last Name</Form.Label>
                    <FormControl type="text"
                                 value={ this.state.lastName }
                                 name="lastName"
                                 onChange={ onChange } />
                </Form.Group>
                <Form.Group>
                    <Form.Label>Country</Form.Label>
                    <FormControl type="text"
                                 value={ this.state.country }
                                 name="country"
                                 onChange={ onChange } />
                </Form.Group>
                <Form.Group>
                    <Form.Label>City</Form.Label>
                    <FormControl type="text"
                                 value={ this.state.city }
                                 name="city"
                                 onChange={ onChange } />
                </Form.Group>
                <Form.Group>
                    <Form.Label>Company</Form.Label>
                    <FormControl type="text"
                                 value={ this.state.company }
                                 name="company"
                                 onChange={ onChange } />
                </Form.Group>
                <Form.Group>
                    <Form.Label>Profession</Form.Label>
                    <FormControl type="text"
                                 value={ this.state.profession }
                                 name="profession"
                                 onChange={ onChange } />
                </Form.Group>
                <Form.Group>
                    <Form.Label>Phone</Form.Label>
                    <FormControl type="text"
                                 value={ this.state.phone }
                                 name="phone"
                                 onChange={ onChange } />
                </Form.Group>
                <Button onClick={ updateProfile }>Save</Button>
            </Form>
        )
    }
}

export default Profile;