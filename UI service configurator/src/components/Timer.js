import React from 'react';

const Timer = (props) => {
    const handleItemClick = (e) => {
        props.changeTimer(e.currentTarget.value);
    }
    return (<h5>
        <Select items={props.timers} onItemSelect={handleItemClick} />
    </h5>
    )
};

const Select = ({ items, onItemSelect }) => (
    <select id="selectBox" onChange={onItemSelect} style={{ cursor: 'pointer' }} >
        {
            items.map((item, i) => <option key={i} value={item.ms}>{item.name}</option>)
        }
    </select>
);

export default Timer;