import React from "react";
import DocKickGoogleLogin from "./auth/google/doc-kick-google-login";
import DocKickGoogleLogout from "./auth/google/doc-kick-google-logout";

class Hello extends React.Component {
    render() {
        return <h1>Привет, React.JS</h1>;
    }
}

const App = React.memo(() => (
    <div>
        <Hello/>
        <DocKickGoogleLogin/>
        <DocKickGoogleLogout/>
    </div>
));

export default App;