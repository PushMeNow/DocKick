import React, { Component } from "react";
import axios from "axios";
import { combineCategorizableUrl } from "../../url-helper";
import { LoaderContext } from "../../context/loader-context";
import SortableTree from "react-sortable-tree";
import 'react-sortable-tree/style.css';

export class CategoryTree extends Component {
    constructor(props) {
        super(props);

        this.state = {
            treeData: []
        };
    }

    componentDidMount() {
        this.fillData();
    }

    fillData() {
        let loaderContext = this.context;

        const parseCategory = (category) => {
            let result = {
                title: category.name,
                children: []
            };

            if (category.children && category.children.length) {
                for (let child of category.children) {
                    result.children.push(parseCategory(child));
                }
            }

            return result;
        }

        axios.get(combineCategorizableUrl('categories'), {
            before: loaderContext.showLoader,
        }).then(response => {
            const treeData = [];
            const categories = response.data;

            for (let category of categories) {
                treeData.push(parseCategory(category));
            }

            this.setState({ treeData });
        }).finally(loaderContext.hideLoader);
    }

    render() {
        return (
            <div style={ { height: 400 } }>
                <SortableTree treeData={ this.state.treeData }
                              onChange={ treeData => this.setState({ treeData }) } />
            </div>
        );
    }
}

CategoryTree.contextType = LoaderContext;