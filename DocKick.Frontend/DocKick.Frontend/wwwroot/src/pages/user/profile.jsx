import React, { Component } from "react";
import axios from "axios";
import { combineIdentityServerUrl } from "../../url-helper";
import { Button, Form, FormControl } from "react-bootstrap";
import { toast } from "react-toastify";
import { LoaderContext } from "../../context/loader-context";

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
        const { showLoader, hideLoader } = this.context;

        axios.get(combineIdentityServerUrl('account/profile'), {
            before: showLoader
        })
             .then(response => {
                 this.setState({ ...response.data, disabled: false });
             })
             .finally(hideLoader);
    }

    render() {
        const { showLoader, hideLoader } = this.context;

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
            }, {
                          before: showLoader
                      }).then(response => {
                this.setState({ ...response.data, disabled: false });

                toast('Profile was saved successfully.', {
                    type: 'success'
                });
            }).finally(hideLoader);
        };

        // const checkCategorizable = () => {
        //     axios.get(combineCategorizableUrl('category/api'))
        //          .then(response => {
        //              alert(response.data);
        //          });
        // }

        const onChange = () => (target) => {
            this.setState({ [target.name]: target.value });
        }

        return (
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
                        onClick={ updateProfile }
                        disabled={ this.state.disabled }>Save</Button>
            </Form>
        )
    }
}

Profile.contextType = LoaderContext;

export default Profile;