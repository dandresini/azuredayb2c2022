import React from "react";

/**
 * Renders information about the user obtained from MS Graph
 * @param props 
 */
export const ProfileData = (props) => {
    console.log(props.graphData);

    return (
        <div id="profile-div">
            <p><strong>Nome: </strong> {props.graphData.name}</p>
            <p><strong>Cognome: </strong> {props.graphData.surname}</p>
            <p><strong>Città: </strong> {props.graphData.city}</p>
            <p><strong>Email: </strong> {props.graphData.email}</p>
            <p><strong>Result:</strong>{typeof (props.graphData) == "object" ? props.graphData.name : props.graphData }</p>
        </div>
    );
};