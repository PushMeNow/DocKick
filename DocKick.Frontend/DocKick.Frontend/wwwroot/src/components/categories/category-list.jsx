import React, { Component } from "react";
import axios from "axios";
import { combineCategorizableUrl } from "../../url-helper";
import { LoaderContext } from "../../context/loader-context";
import { Button, Table } from "react-bootstrap";
import { AddCategoryButton } from "./add-category-button";

export class CategoryList extends Component {
    constructor(props) {
        super(props);

        this.state = {
            data: null
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
            <td colSpan={ 2 }>Nothing found</td>
        </tr>
    }

    loadData() {
        let loaderContext = this.context;

        axios.get(combineCategorizableUrl('categories'), {
            before: loaderContext.showLoader,
        }).then(response => {
            this.setState({ data: response.data });
        }).finally(loaderContext.hideLoader);
    }

    renderTableBody() {
        return (!this.state.data.length
            ? this.renderNothingFound()
            : this.state.data.map(item => (
                <tr>
                    <td>
                        { item.name }
                    </td>
                    <td align="center">
                        <Button variant="secondary">Edit</Button>
                        <Button variant="danger">Delete</Button>
                    </td>
                </tr>
            )));
    }

    render() {
        return (
            <>
                <div className="mb-3 d-flex justify-content-end">
                    <AddCategoryButton updateTable={ this.loadData } />
                </div>
                <Table striped
                       bordered
                       hover>
                    <thead>
                    <tr>
                        <td>Name</td>
                        <td>Action</td>
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

CategoryList.contextType = LoaderContext;