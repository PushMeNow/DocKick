import React, { Component } from "react";
import { Button, FormControl, FormLabel, Modal, ModalBody, ModalTitle } from "react-bootstrap";
import ModalHeader from "react-bootstrap/ModalHeader";

export default class CategoryModalForm extends Component {
    constructor(props) {
        super(props);

        this.state = {
            show: false
        };

        this.showModal = this.showModal.bind(this);
        this.hideModal = this.hideModal.bind(this);
    }

    showModal() {
        this.setState({ show: true });
    }

    hideModal() {
        this.setState({ show: false });
    }

    render() {
        const onChange = (event) => {
            this.setState({ [event.target.name]: event.target.value });
        }

        return <Modal show={ this.state.show }
                      onHide={ this.hideModal }>
            <ModalHeader closeButton>
                <ModalTitle>Add category</ModalTitle>
            </ModalHeader>
            <ModalBody>
                <FormLabel>Name</FormLabel>
                <FormControl type="text"
                             placeholder="Enter category name"
                             name="categoryName"
                             value={ this.state.categoryName }
                             onChange={ onChange } />
            </ModalBody>
            <Modal.Footer>
                <Button variant="primary"
                        onClick={ addCategory }>Save</Button>
                <Button variant="secondary"
                        onClick={ hideModal }>Close</Button>
            </Modal.Footer>
        </Modal>
    }
}