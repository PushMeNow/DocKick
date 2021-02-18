import React, { Component } from "react";
import { Link } from "react-router-dom";

export class DropdownLink extends Component {
    constructor(props) {
        super(props);
    }

    render() {
        return <Link { ...this.props } className="dropdown-item">{ this.props.children }</Link>
    }
}