import React, { Component } from "react";
import { ClipLoader } from "react-spinners";
import "../../content/loader.css";

export class Loader extends Component {
    render() {
        let { showLoader } = this.props;

        return (
            <div className={showLoader ? "dockick-loader-background" : "hidden"}>
                <div className="dockick-loader">
                    <ClipLoader size={ 150 }
                                loading={ showLoader } />
                </div>
            </div>
        )
    }
}