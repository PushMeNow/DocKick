import React, { Component } from "react";
import { Button, FormControl, FormLabel, Modal, ModalBody, ModalTitle } from "react-bootstrap";
import ModalHeader from "react-bootstrap/ModalHeader";
import axios from "axios";
import { combineCategorizableUrl } from "../../url-helper";
import { toast } from "react-toastify";

export default class CategoryModalForm extends Component {
    constructor(props) {
        super(props);

        this.state = {
            show: false,
            action: 'add',
            ...this.getDefaultCategory()
        };

        this.setAddAction = () => this.setState({ action: 'add' });
        this.setEditAction = () => this.setState({ action: 'edit' });

        this.showModal = this.showModal.bind(this);
        this.hideModal = this.hideModal.bind(this);
    }

    showModal() {
        this.setState({ show: true });
    }

    hideModal() {
        this.setState({ show: false });
    }

    setCategory(category) {
        if (!category || !category.categoryId) {
            this.setState({ ...this.getDefaultCategory() });
            this.setAddAction();

            return;
        }

        let { categoryId, name, parentId } = category;

        this.setEditAction();
        this.setState({ categoryId, name, parentId });
    }

    getDefaultCategory() {
        return {
            categoryId: null,
            name: '',
            parentId: null
        };
    }

    getCategoryData() {
        let { show, action, ...category } = this.state;

        return category;
    }

    render() {
        const onChange = (event) => {
            this.setState({ [event.target.name]: event.target.value });
        }

        const saveCategory = () => {
            const { updateTable } = this.props;
            let category = this.getCategoryData(),
                isAdd = this.state.action === 'add';

            axios({
                      method: (isAdd ? 'post' : 'put'),
                      data: { ...category },
                      url: combineCategorizableUrl('categories' + (isAdd ? '' : `/${ category.categoryId }`))
                  })
                .then(response => {
                    this.hideModal();
                    toast(`Category '${ response.data.name }' was saved successfully.`, {
                        type: "success"
                    });
                })
                .finally(() => {
                    updateTable();
                });
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
                             name="name"
                             value={ this.state.name }
                             onChange={ onChange } />
            </ModalBody>
            <Modal.Footer>
                <Button variant="primary"
                        onClick={ saveCategory }>Save</Button>
                <Button variant="secondary"
                        onClick={ this.hideModal }>Close</Button>
            </Modal.Footer>
        </Modal>
    }
}