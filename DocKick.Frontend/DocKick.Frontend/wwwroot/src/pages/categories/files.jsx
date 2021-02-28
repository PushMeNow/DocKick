import React, { Component, createRef } from "react";
import { Button, ButtonGroup, Form, Image, Table } from "react-bootstrap";
import FormFileInput from "react-bootstrap/FormFileInput";
import FormFileLabel from "react-bootstrap/FormFileLabel";
import axios from "axios";
import { combineCategorizableUrl } from "../../url-helper";
import { toastError, toastSuccess } from "../../helpers/toast-helpers";

export default class FilesPage extends Component {
    constructor(props) {
        super(props);

        this.fileLabel = createRef();
        this.fileInput = createRef();

        this.state = {
            data: null
        }
    }

    renderNothingFound() {
        return <tr>
            <td colSpan={ 3 }>
                You don't have any documents.
            </td>
        </tr>
    }

    renderTableBody() {
        return (
            !this.state.data.length
                ? this.renderNothingFound()
                : this.state.data.map(blob => {
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

                    return <tr>
                        <td>{ blob.name }</td>
                        <td className="text-center">
                            <a href={ blob.blobLink.url }
                               target="_blank">
                                <Image rounded
                                       src={ `${ blob.blobLink.url }` }
                                       width={ 300 } />
                            </a>
                        </td>
                        <td>
                            <Button type="button"
                                    variant="danger"
                                    onClick={ deleteImage }>
                                Delete
                            </Button>
                        </td>
                    </tr>
                })
        );
    }

    loadBlobs() {
        axios.get(combineCategorizableUrl('blobs'))
             .then(response => {
                 this.setState({ data: response.data });
             });
    }

    componentDidMount() {
        if (this.state.data === null) {
            this.loadBlobs();
        }
    }

    render() {
        const labelText = 'Here will be your file name';
        const onChange = (e) => {
            let filePath = e.target.value;

            this.fileLabel.current.innerText = !filePath ? labelText : filePath.split('\\').pop();
        };
        const upload = () => {
            if (!this.fileInput.current.files.length) {
                this.fileInput.isValid = false;

                return false;
            }

            let file = this.fileInput.current.files[0];
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

        return (
            <>
                <div>
                    <Form>
                        <ButtonGroup>
                            <Form.File custom>
                                <FormFileInput id="file-browser"
                                               onChange={ onChange }
                                               ref={ this.fileInput } />
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
                </div>
                <div className="mt-3">
                    <Table striped
                           bordered
                           hover>
                        <thead>
                        <tr>
                            <th>Name</th>
                            <th>Image</th>
                            <th>Action</th>
                        </tr>
                        </thead>
                        <tbody>
                        {
                            this.state.data === null
                                ? this.renderNothingFound()
                                : this.renderTableBody()
                        }
                        </tbody>
                    </Table>
                </div>
            </>
        )
    }
}