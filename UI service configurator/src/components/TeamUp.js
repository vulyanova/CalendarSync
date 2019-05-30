import React from 'react';
import CalendarKey from '../containers/calendarKeyContainer'

const TeamUp = () => {
    return (
    <div>
        <table className='table'>
            <thead>
                <tr>
                    <th>TeamUp Calendar Key</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td><CalendarKey /></td>
                </tr>
            </tbody>
        </table>      
    </div>
)
};


export default TeamUp