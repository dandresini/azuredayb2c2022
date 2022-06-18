import { protectedResources } from "./authConfig";

/**
 * Attaches a given access token to a MS Graph API call. Returns information about the user
 * @param accessToken 
 */
export async function callMsGraph(accessToken) {
    const headers = new Headers();
    const bearer = `Bearer ${accessToken}`;

    headers.append("Authorization", bearer);
    headers.append("Accept", 'application/json');
    headers.append("Content-Type", 'application/json');
    
    const options = {
        method: "GET",
        headers: headers
    };

    return fetch(protectedResources.azureFunction.endpoint, options)
        .then(response => response.json())
        .catch(error => console.log(error));
}
