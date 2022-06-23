/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

import { LogLevel } from "@azure/msal-browser";

export const b2cPolicies = {
    names: {
        signUpSignIn: "b2c_1_susi",
        forgotPassword: "b2c_1_reset",
        editProfile: "b2c_1_edit_profile"
    },
    authorities: {
        signUpSignIn: {
            authority: "https://azuredayIta.b2clogin.com/azuredayIta.onmicrosoft.com/b2c_1_susi",
        },
        forgotPassword: {
            authority: "https://azuredayIta.b2clogin.com/azuredayIta.onmicrosoft.com/b2c_1_reset",
        },
        editProfile: {
            authority: "https://azuredayIta.b2clogin.com/azuredayIta.onmicrosoft.com/b2c_1_edit_profile"
        }
    },
    authorityDomain: "azuredayIta.b2clogin.com"
}

/**
 * Configuration object to be passed to MSAL instance on creation. 
 * For a full list of MSAL.js configuration parameters, visit:
 * https://github.com/AzureAD/microsoft-authentication-library-for-js/blob/dev/lib/msal-browser/docs/configuration.md 
 */
export const msalConfig = {
    auth: {
        clientId: "3921f4fc-1e8c-45f1-947e-a1fffb0e861c", // This is the ONLY mandatory field that you need to supply.
        authority: b2cPolicies.authorities.signUpSignIn.authority, // Choose SUSI as your default authority.
        knownAuthorities: [b2cPolicies.authorityDomain], // Mark your B2C tenant's domain as trusted.
        redirectUri: "/", // You must register this URI on Azure Portal/App Registration. Defaults to window.location.origin
        postLogoutRedirectUri: "/", // Indicates the page to navigate after logout.
        navigateToLoginRequestUrl: false, // If "true", will navigate back to the original request location before processing the auth code response.
    },
    cache: {
        cacheLocation: "sessionStorage", // This configures where your cache will be stored
        storeAuthStateInCookie: false, // Set this to "true" if you are having issues on IE11 or Edge
    },
    system: {	
        loggerOptions: {	
            loggerCallback: (level, message, containsPii) => {	
                if (containsPii) {		
                    return;		
                }		
                switch (level) {		
                    case LogLevel.Error:		
                        console.error(message);		
                        return;		
                    case LogLevel.Info:		
                        console.info(message);		
                        return;		
                    case LogLevel.Verbose:		
                        console.debug(message);		
                        return;		
                    case LogLevel.Warning:		
                        console.warn(message);		
                        return;		
                }	
            }	
        }	
    }
};

export const protectedResources = {
    azureFunction: {
        endpoint: "https://functionb2ctest.azurewebsites.net/api/UserInformation?code=6i0XXIlwSScmcwrmyaNcKQBXFz_lWkMK5_5B-AMt5ZoKAzFuytAlrA==",
        scopes: ["https://azuredayIta.onmicrosoft.com/d5d55679-722e-4318-947c-027f44150c82/function.read"], // e.g. api://xxxxxx/access_as_user
    },
}

/**
 * Scopes you add here will be prompted for user consent during sign-in.
 * By default, MSAL.js will add OIDC scopes (openid, profile, email) to any login request.
 * For more information about OIDC scopes, visit: 
 * https://docs.microsoft.com/en-us/azure/active-directory/develop/v2-permissions-and-consent#openid-connect-scopes
 */
 export const loginRequest = {
    scopes: [...protectedResources.azureFunction.scopes]
};
