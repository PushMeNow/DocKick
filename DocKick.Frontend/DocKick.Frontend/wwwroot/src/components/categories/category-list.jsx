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
            data: null
        }

        this.modal = null;
        this.setModal = el => {
            this.modal = el;
        }

        this.loadData = this.loadData.bind(this);
    }

    componentDidMount() {
        if (this.state.data === null) {
            this.loadData();
        }
    }

    renderNothingFound() {
        return <tr>
            <td colSpan={ 3 }>Nothing found</td>
        </tr>
    }

    loadData() {
        axios.get(combineCategorizableUrl('categories'))
             .then(response => {
                 this.setState({ data: response.data });
             });
    }

    renderTableBody() {
        return (!this.state.data.length
            ? this.renderNothingFound()
            : this.state.data.map(category => {
                const showModal = () => {
                    this.modal.setCategory(category);
                    this.modal.showModal();
                }

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
                }

                return (
                    <tr>
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
    }

    render() {
        const showModal = () => {
            this.modal.setCategory();
            this.modal.showModal();
        }

        return (
            <>
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
                    { this.state.data === null
                        ? this.renderNothingFound()
                        : this.renderTableBody() }
                    </tbody>
                </Table>
            </>
        )
    }
}