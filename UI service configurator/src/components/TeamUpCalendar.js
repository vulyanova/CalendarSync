import React from 'react';

const TeamUpCalendar = (props) => {
    const handleItemClick = (e) => {
        props.changeTeamUpCalendar(e.currentTarget.value);
    }
    return (
        <h5>
            <Select items={props.teamUpCalendars} onItemSelect={handleItemClick} />
        </h5>
    )
};

const Select = ({ items, onItemSelect }) => (
    <select id="selectBox" onChange={onItemSelect} style={{ cursor: 'pointer' }} >
        {
            items.map((item, i) => <option key={i} value={item.id}>{item.name} </option>)
        }
    </select>
);

export default TeamUpCalendar;