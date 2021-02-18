import React, { Component, createContext } from "react";
import { Loader } from "../components/loader/loader";

export const LoaderContext = createContext({});

export class LoaderProvider extends Component {
    constructor(props) {
        super(props);
        this.state = {
            loading: false
        }
    }

    showLoader = () => {
        this.setState({ loading: true });
    }

    hideLoader = () => {
        this.setState({ loading: false });
    }

    render() {
        const providerValues = {
            showLoader: this.showLoader,
            hideLoader: this.hideLoader
        }

        const { loading } = this.state;

        return (
            <LoaderContext.Provider value={ providerValues }>
                <Loader showLoader={ loading } />
                { this.props.children }
            </LoaderContext.Provider>
        );
    }
}