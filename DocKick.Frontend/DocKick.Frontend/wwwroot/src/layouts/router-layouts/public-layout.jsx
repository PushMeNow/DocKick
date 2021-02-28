import React from 'react';
import { Route, Switch } from 'react-router-dom';
import { LoginCallback } from "../../components/auth/login-callback";
import { LogoutCallback } from "../../components/auth/logout-callback";
import Profile from "../../pages/user/profile";
import { CategoryPage } from "../../pages/categories/category";
import { CategoryTree } from "../../pages/categories/category-tree";
import FilesPage from "../../pages/categories/files";

const PublicLayout = () => {
    return (
        <Switch>
            <Route path="/login-callback"
                   component={ LoginCallback } />
            <Route path="/logout-callback"
                   component={ LogoutCallback } />
            <Route path="/profile"
                   component={ Profile } />
            <Route path="/categories"
                   component={ CategoryPage } />
            <Route path="/category-tree"
                   component={ CategoryTree } />
            <Route path="/files"
                   component={ FilesPage } />
        </Switch>
    )
}

export default PublicLayout;