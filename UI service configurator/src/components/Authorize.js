import React from 'react';
import Editing from '../components/Editing';
import Users from '../containers/usersContainer'

const Authorize = (props) => {
    return (
        <div>
            <Users/>
            <Editing/>
            <button onClick={() => {
                props.authorize(props.authorizationParams);
            }}>
                Authorize
            </button>
        </div>
    )
}

export default Authorize;