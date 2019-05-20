import React from 'react';

const Calendar = (props) => {
    const handleItemClick = (e) => {
        props.changeCalendar(e.currentTarget.value);
    }
    return (
        <h5>
            <Select items={props.calendars} onItemSelect={handleItemClick} />
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

export default Calendar;