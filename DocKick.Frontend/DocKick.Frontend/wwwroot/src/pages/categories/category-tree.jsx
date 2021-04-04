import React, { Component } from "react";
import axios from "axios";
import { combineCategorizableUrl } from "../../url-helper";
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
        const parseCategory = category => {
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

        axios.get(combineCategorizableUrl('categories/category-tree'))
             .then(response => {
                 const treeData = [];
                 const categories = response.data;

                 for (let category of categories) {
                     treeData.push(parseCategory(category));
                 }

                 this.setState({ treeData });
             });
    }

    changeNodeData(newNodeData) {
        axios.put(combineCategorizableUrl(`categories/${ newNodeData.categoryId }`), {
            ...newNodeData
        });
    }

    render() {
        const onTreeChange = treeData => this.setState({ treeData });
        const onMoveNode = ({ nextParentNode, node }) => {
            let newNodeData = {
                ...node.currentData,
                parentId: nextParentNode ? nextParentNode.currentData.categoryId : null
            }
            this.changeNodeData(newNodeData);
        };

        const renderTree = () => {
            if (!this.state.treeData || !this.state.treeData.length) {
                return <div>There aren't tree nodes.</div>
            }

            return <SortableTree treeData={ this.state.treeData }
                                 onChange={ onTreeChange }
                                 onMoveNode={ onMoveNode } />;
        }

        return (
            <div style={ { height: 400 } }>
                { renderTree() }
            </div>
        );
    }
}