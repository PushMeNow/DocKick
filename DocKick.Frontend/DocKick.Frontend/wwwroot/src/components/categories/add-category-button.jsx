import React, { Component } from "react";
import { Button, FormControl, FormLabel, Modal, ModalBody, ModalTitle } from "react-bootstrap";
import ModalHeader from "react-bootstrap/ModalHeader";
import { LoaderContext } from "../../context/loader-context";
import axios from "axios";
import { combineCategorizableUrl } from "../../url-helper";

export class AddCategoryButton extends Component {
    constructor(props) {
        super(props);

        this.state = {
            show: false,
            categoryName: ''
        };
    }

    render() {
        const showModal = () => {
            this.setState({ show: true });
        }

        const hideModal = () => {
            this.setState({ show: false });
        }

        const onChange = (event) => {
            this.setState({ [event.target.name]: event.target.value });
        }

        const addCategory = () => {
            const { updateTable } = this.props;
            const loaderContext = this.context

            axios.post(combineCategorizableUrl('categories'), { name: this.state.categoryName }, {
                before: loaderContext.showLoader
            }).then(response => {
                hideModal();
            }).finally(() => {
                loaderContext.hideLoader();
                updateTable();
            });
        }

        return (
            <>
                <Button variant="success"
                        onClick={ showModal }>Add Category</Button>

                <Modal show={ this.state.show }
                       onHide={ hideModal }>
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
            </>
        )
    }
}

AddCategoryButton.contextType = LoaderContext;