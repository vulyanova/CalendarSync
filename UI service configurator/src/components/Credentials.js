import React from 'react';

const Credentials = (props)=>{
    let input;
    return (<h5>
        <input type="file" accept=".json" ref={node => {
            input = node;
        }} onChange={() => {
            const reader = new FileReader();
            reader.onload = function (evt) {
                const credentials = JSON.parse(evt.target.result);
                props.changeClientId(credentials.installed.client_id);
                props.changeClientSecret(credentials.installed.client_secret);
            }
            reader.readAsText(input.files[0]); 
            }}/>          
        </h5>
)
};


export default Credentials