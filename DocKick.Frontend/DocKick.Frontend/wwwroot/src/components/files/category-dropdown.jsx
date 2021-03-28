import React, { Component } from "react";
import axios from "axios";
import { combineCategorizableUrl } from "../../url-helper";
import { toastSuccess } from "../../helpers/toast-helpers";
import { Form } from "react-bootstrap";

export class CategoryDropdown extends Component {
    constructor(props) {
        super(props);

        if (!props.categories) {
            throw 'Category list cannot be empty.';
        }

        if (!props.blob) {
            throw 'Blob cannot be empty.';
        }
    }

    render() {
        const blob = this.props.blob;
        const categories = this.props.categories;

        const onChange = ({ target }) => {
            let value = target.value;

            if (!blob) {
                console.error('Incorrect data.');

                return false;
            }

            axios.put(combineCategorizableUrl(`blobs/${ blob.blobId }`), {
                categoryId: value || null
            }).then(response => {
                toastSuccess(`Category was ${ !value ? 'removed' : 'updated' } successfully for ${ blob.name }.`);
            });
        };

        return (
            <Form.Control as="select"
                          custom
                          onChange={ onChange }
                          defaultValue={ blob.categoryId || '' }>
                <option value="">Nothing selected</option>
                {
                    categories && categories.length && categories.map(category => {
                        return <option key={ 'categories-list-' + blob.blobId + category.categoryId }
                                       value={ category.categoryId }>{ category.name }</option>
                    })
                }
            </Form.Control>
        )
    }
}