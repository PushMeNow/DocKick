import React, { Component } from "react";
import axios from "axios";
import { combineCategorizableUrl } from "../../url-helper";
import { Button, Table } from "react-bootstrap";
import CategoryModalForm from "./category-modal-form";
import { toastSuccess } from "../../helpers/toast-helpers";

export class CategoryList extends Component {
    constructor(props) {
        super(props);

        this.state = {
            categories: null
        };

        this.modal = null;
        this.setModal = el => {
            this.modal = el;
        };

        this.loadData = this.loadData.bind(this);
    }

    componentDidMount() {
        if (this.state.categories === null) {
            this.loadData();
        }
    }


    loadData() {
        axios.get(combineCategorizableUrl('categories'))
             .then(response => {
                 this.setState({ categories: response.data });
             });
    }


    render() {
        const showModal = () => {
            this.modal.setCategory();
            this.modal.showModal();
        };

        const renderNothingFound = () => {
            return <tr>
                <td colSpan={ 3 }>Nothing found</td>
            </tr>
        };

        const renderTableBody = (categories) => {
            return (
                !categories || !categories.length
                    ? renderNothingFound()
                    : categories.map(category => {
                        const showModal = () => {
                            this.modal.setCategory(category);
                            this.modal.showModal();
                        };

                        const deleteCategory = () => {
                            if (!confirm(`Are you sure want delete category ${ category.name }?`)) {
                                return;
                            }

                            axios.delete(combineCategorizableUrl(`categories/${ category.categoryId }`))
                                 .then(() => {
                                     toastSuccess(`Category ${ category.name } was deleted successfully.`);
                                 }).finally(() => {
                                this.loadData();
                            });
                        };

                        return (
                            <tr key={ 'category-list-' + category.categoryId }>
                                <td>
                                    { category.name }
                                </td>
                                <td>
                                    { category.parentName }
                                </td>
                                <td align="center">
                                    <Button variant="secondary"
                                            onClick={ showModal }>Edit</Button>
                                    <Button variant="danger"
                                            onClick={ deleteCategory }>Delete</Button>
                                </td>
                            </tr>
                        )
                    }));
        };

        return (
            <div>
                <div className="mb-3 d-flex justify-content-end">
                    <CategoryModalForm ref={ this.setModal }
                                       updateTable={ this.loadData } />
                    <Button variant="success"
                            onClick={ showModal }>Add Category</Button>
                </div>
                <Table striped
                       bordered
                       hover>
                    <thead>
                    <tr>
                        <th>Name</th>
                        <th>Parent</th>
                        <th>Action</th>
                    </tr>
                    </thead>
                    <tbody>
                    { this.state.categories === null
                        ? renderNothingFound()
                        : renderTableBody(this.state.categories) }
                    </tbody>
                </Table>
            </div>
        )
    }
}