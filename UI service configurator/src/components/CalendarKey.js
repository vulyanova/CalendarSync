import React from 'react';

const CalendarKey = ({ changeCalendarKey}) =>
{    
    let input;
    return (
        <h5>
            <input ref={node => {
                input = node;
            }}
            onInput={() => {
                changeCalendarKey(input.value)
            }} >
            </input> 
        </h5>
    )
}

export default CalendarKey;