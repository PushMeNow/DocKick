import React, { Component } from "react";
import axios from "axios";
import { combineCategorizableUrl, combineIdentityServerUrl } from "../../url-helper";
import { Button, Form, FormControl, FormLabel } from "react-bootstrap";

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
            phone: '',
            disabled: true
        }

        this.submitBtn = React.createRef();
    }

    componentDidMount() {
        axios.get(combineIdentityServerUrl('account/profile'))
             .then(response => {
                 this.setState({ ...response.data, disabled: false });
             });
    }

    render() {
        const onClick = () => {
            let { disabled, email, userId, ...user } = this.state;

            axios.put(combineIdentityServerUrl('account/profile'), { ...user })
                 .then(response => {
                     this.setState({ ...response.data, disabled: false });
                 });
        };

        const checkCategorizable = () => {
            axios.get(combineCategorizableUrl('category/api'))
                 .then(response => {
                     alert(response.data);
                 });
        }

        const onChange = (event) => {
            this.setState({ [event.target.name]: event.target.value });
        }

        return (
            <>
                <Form>
                    <Form.Group>
                        <Form.Label>Email</Form.Label>
                        <FormControl type="text"
                                     value={ this.state.email }
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
                    <Button ref={ this.submitBtn }
                            onClick={ onClick }
                            disabled={ this.state.disabled }>Save</Button>
                </Form>
                <Button onClick={ checkCategorizable }>Check Authorized External API</Button>
            </>
        )
    }
}

export default Profile;