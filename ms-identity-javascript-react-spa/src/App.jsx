import React, { useState } from "react";
import { AuthenticatedTemplate, UnauthenticatedTemplate, useMsal } from "@azure/msal-react";
import { loginRequest, b2cPolicies } from "./authConfig";
import { PageLayout } from "./components/PageLayout";
import { ProfileData } from "./components/ProfileData";
import { callMsGraph } from "./graph";
import Button from "react-bootstrap/Button";
import "./styles/App.css";

/**
 * Renders information about the signed-in user or a button to retrieve data about the user
 */
const ProfileContent = () => {
    const { instance, accounts } = useMsal();
    const [graphData, setGraphData] = useState(null);

    function RequestProfileData() {
        const silentRequest = {
            scopes: loginRequest.scopes,
            loginHint: accounts[0].username
        };
        //------------------------------
        //prima di chiamare la pagina di profilo richiedo un nuovo access token da passare
        //------------------------------
        try {
            instance.ssoSilent(silentRequest).then(
                response=> callMsGraph(response.accessToken).then(response => setGraphData(response))
            );
        } catch (err) {
            if (err instanceof InteractionRequiredAuthError) {
                instance.loginPopup(silentRequest);
            } else {
                console.log("Errore dopo Edit profile:"+ err);
            }
        }
    };

    return (
        <>
            <h5 className="card-title">Welcome {accounts[0].name}</h5>
            {graphData ? 
                <ProfileData graphData={graphData} />
                :
                <>
                    <Button variant="info" onClick={() => instance.loginPopup(b2cPolicies.authorities.editProfile)} className="ml-auto">Edit Profile</Button>
                    <br/><br/>
                    <Button variant="secondary" onClick={RequestProfileData}>Request Profile Information</Button>
                </>

            }

        </>
    );
};

/**
 * If a user is authenticated the ProfileContent component above is rendered. Otherwise a message indicating a user is not authenticated is rendered.
 */
const MainContent = () => {    
    return (
        <div className="App">
            <AuthenticatedTemplate>
                <ProfileContent />
            </AuthenticatedTemplate>

            <UnauthenticatedTemplate>
                <h5 className="card-title">Please sign-in to see your profile information.</h5>
            </UnauthenticatedTemplate>
        </div>
    );
};

export default function App() {
    return (
        <PageLayout>
            <MainContent />
        </PageLayout>
    );
}
