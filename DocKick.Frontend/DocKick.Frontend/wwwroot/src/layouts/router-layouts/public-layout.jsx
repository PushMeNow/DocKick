import React from 'react';
import { Route, Switch } from 'react-router-dom';
import Profile from "../../pages/user/profile";
import { CategoryPage } from "../../pages/categories/category";
import { CategoryTree } from "../../pages/categories/category-tree";
import FilesPage from "../../pages/categories/files";
import { LoginCallbackPage } from "../../pages/auth/login-callback-page";
import { LogoutCallbackPage } from "../../pages/auth/logout-callback-page";

const PublicLayout = () => {
    return (
        <Switch>
            <Route path="/login-callback"
                   component={ LoginCallbackPage } />
            <Route path="/logout-callback"
                   component={ LogoutCallbackPage } />
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