import React from 'react';

const List = ({ items, onItemClick }) => (
        <ul style={{ cursor: 'pointer' }}>
            {
                items.map((item, i) =>
                {
                    const boundItemClick = onItemClick.bind(this, item);
                    return <li key={i} value={item} onClick={boundItemClick}>{item}</li>
                })
            }
        </ul>
);

const Users = (props) => {
    if (!props.usersFetched)
        return <div>
                <button onClick={() => {
                    props.getUsers();
                }}>
                    Get authorized users
                </button>
            </div>
    const handleItemClick = (item, e) => {
        const user = item;
        props.changeUser(user);
        props.stopAuthorizing();
        props.getCalendars(user);
    }
    return <List items={props.users} onItemClick={handleItemClick} />
}


export default Users;

