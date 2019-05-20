import React from 'react';

const User = ({ changeUser }) => {
    let input;
    return (<h5>
        <input ref={node => {
            input = node;
        }}
            onInput={() => {
                changeUser(input.value)
            }} >
        </input> 
        </h5>
)
};

export default User