import React from 'react';

const ShowSummary = ({ changeSummary} )=>{
    let input;
    return(
        <h5>
                <input type="checkbox"
                    ref={node => {
                        input = node;
                    }}
                    onClick={() => {
                        changeSummary(input.checked)
                    }}>
            </input>
        </h5>)
}


export default ShowSummary;