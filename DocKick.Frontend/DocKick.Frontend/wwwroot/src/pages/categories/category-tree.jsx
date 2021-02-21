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

        this.fillData = this.fillData.bind(this);
        this.changeNodeData = this.changeNodeData.bind(this);
    }

    componentDidMount() {
        this.fillData();
    }

    fillData() {
        const loaderContext = this.context;

        const parseCategory = (category) => {
            let result = {
                title: category.name,
                expanded: true,
                currentData: category,
                children: []
            };

            if (category.children && category.children.length) {
                for (let child of category.children) {
                    result.children.push(parseCategory(child));
                }
            }

            return result;
        }

        axios.get(combineCategorizableUrl('categories/category-tree'), {
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

    changeNodeData(newNodeData) {
        const loaderContext = this.context;

        axios.put(combineCategorizableUrl(`categories/${ newNodeData.categoryId }`), {
            ...newNodeData
        }, {
                      before: loaderContext.showLoader
                  }).finally(loaderContext.hideLoader);
    }

    render() {
        const onTreeChange = (treeData) => this.setState({ treeData });
        const onMoveNode = ({ nextParentNode, node }) => {
            let newNodeData = {
                ...node.currentData,
                parentId: nextParentNode ? nextParentNode.currentData.categoryId : null
            }
            this.changeNodeData(newNodeData);
        };

        return (
            <div style={ { height: 400 } }>
                <SortableTree treeData={ this.state.treeData }
                              onChange={ onTreeChange }
                              onMoveNode={ onMoveNode } />
            </div>
        );
    }
}

CategoryTree.contextType = LoaderContext;