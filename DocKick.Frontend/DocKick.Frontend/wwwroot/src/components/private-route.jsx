import React, { useContext } from 'react';
import PropTypes from 'prop-types';
import { Route } from 'react-router-dom';

const PrivateRoute = ({ component: Component, ...rest }) => {
    return (
        <Route
            {...rest}
            render={({ location, ...props }) => {
                if (!!Component) {
                    return <Component {...props} />;
                }
            }}
        />
    );
};

export default PrivateRoute;
