import React, { Component, createRef } from "react";
import { Button, ButtonGroup, Form, Image, Table } from "react-bootstrap";
import FormFileInput from "react-bootstrap/FormFileInput";
import FormFileLabel from "react-bootstrap/FormFileLabel";
import axios from "axios";
import { combineCategorizableUrl } from "../../url-helper";
import { toastError, toastSuccess } from "../../helpers/toast-helpers";
import { isSupportedImage, supportedImageTypes } from "../../helpers/file-type-helpers";
import { CategoryDropdown } from "../../components/files/category-dropdown";

export default class FilesPage extends Component {
    constructor(props) {
        super(props);

        this.fileLabel = createRef();
        this.fileInput = createRef();

        this.state = {
            blobs: null,
            categories: null
        }
        
        this.loadBlobs = this.loadBlobs.bind(this);
        this.loadCategories = this.loadCategories.bind(this);
    }    

    loadBlobs() {
        axios.get(combineCategorizableUrl('blobs'))
             .then(response => {
                 this.setState({ blobs: response.data });
             });
    }

    loadCategories() {
        axios.get(combineCategorizableUrl('categories'))
             .then(response => {
                 this.setState({ categories: response.data });
             });
    }

    componentDidMount() {
        if (this.state.categories === null) {
            this.loadCategories();
        }

        if (this.state.blobs === null) {
            this.loadBlobs();
        }
    }

    render() {
        const labelText = 'Here will be your file name';
        const changeFile = ({ target }) => {
            let filePath = target.value;

            this.fileLabel.current.innerText = !filePath ? labelText : filePath.split('\\').pop();
        };
        const upload = () => {
            if (!this.fileInput.current.files.length) {
                this.fileInput.isValid = false;

                return false;
            }

            let file = this.fileInput.current.files[0];

            if (!isSupportedImage(file.name)) {
                toastError('Sorry but your file is not supported.');
                this.fileInput.isValid = false;

                return false;
            }

            let fileName = file.name.split('\\').pop();

            if (this.state.blobs.find(blob => fileName === blob.name)) {
                toastError('Sorry but you already have the same file.');

                return false;
            }

            let data = new FormData();

            data.append('formFile', file);

            axios.post(combineCategorizableUrl('blobs/upload'), data, {
                headers: {
                    'Content-Type': 'multipart/form-data'
                }
            }).then(response => {
                toastSuccess('File was uploaded successfully.');

                this.loadBlobs();
            }).catch(() => {
                toastError('Sorry but we cannot upload your file.');
            });
        };

        const renderNothingFound = () => {
            return <tr>
                <td colSpan={ 4 }>
                    You don't have any documents.
                </td>
            </tr>
        }

        const renderTableBody = (blobs, categories) => {
            return (
                !blobs || !blobs.length
                    ? renderNothingFound()
                    : blobs.map(blob => {
                        const deleteImage = () => {
                            if (!blob || !blob.blobId || !confirm('Are you sure you want to delete this image?')) {
                                return false;
                            }

                            axios.delete(combineCategorizableUrl(`blobs/${ blob.blobId }`))
                                 .then(() => {
                                     toastSuccess('Your image was deleted successfully.');

                                     this.loadBlobs();
                                 });
                        };

                        let ddl = !categories || !categories.length ? '' : <CategoryDropdown key={ 'category-ddl-' + blob.blobId }
                                                                                               categories={ categories }
                                                                                               blob={ blob } />;

                        return <tr key={ 'files-list-' + blob.blobId }>
                            <td>{ blob.name }</td>
                            <td>
                                { ddl }
                            </td>
                            <td className="text-center">
                                <a href={ blob.blobLink.url }
                                   target="_blank">
                                    <Image rounded
                                           src={ `${ blob.blobLink.url }` }
                                           width={ 300 } />
                                </a>
                            </td>
                            <td className="text-center">
                                <Button type="button"
                                        variant="danger"
                                        onClick={ deleteImage }>
                                    Delete
                                </Button>
                            </td>
                        </tr>
                    })
            );
        };

        return (
            <div>
                <Form>
                    <ButtonGroup>
                        <Form.File custom>
                            <FormFileInput id="file-browser"
                                           onChange={ changeFile }
                                           ref={ this.fileInput }
                                           accept={ supportedImageTypes.join(',') } />
                            <FormFileLabel htmlFor="file-browser"
                                           ref={ this.fileLabel }
                                           style={ { overflow: 'hidden' } }>
                                { labelText }
                            </FormFileLabel>
                        </Form.File>
                        <Button variant="primary"
                                type="button"
                                onClick={ upload }>
                            Upload
                        </Button>
                    </ButtonGroup>
                </Form>
                <div className="mt-3">
                    <Table striped
                           bordered
                           hover>
                        <thead>
                        <tr>
                            <th>Name</th>
                            <th>Category</th>
                            <th>Image</th>
                            <th>Action</th>
                        </tr>
                        </thead>
                        <tbody>
                        {
                            this.state.blobs === null
                                ? renderNothingFound()
                                : renderTableBody(this.state.blobs, this.state.categories)
                        }
                        </tbody>
                    </Table>
                </div>
            </div>
        )
    }
}