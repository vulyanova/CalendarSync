import React from 'react';
import Editing from '../components/Editing';
import Users from '../containers/usersContainer'
import TeamUp from '../components/TeamUp'

const Authorize = (props) => {
    return (
        <div>
            <Users/>
            <Editing/>
            <TeamUp/>
            <button onClick={() => {
                props.authorize(props.authorizationParams);
            }}>
                Authorize
            </button>

            <button onClick={() => {
                const user = "vika";
                props.getHistory(user);
            }}>
                Get History
            </button>
        </div>
    )
}

export default Authorize;