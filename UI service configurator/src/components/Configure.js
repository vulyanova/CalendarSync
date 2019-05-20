import React from 'react';
import Timer from '../containers/timerContainer';
import Summary from '../containers/summaryContainer';
import Calendar from '../containers/calendarContainer';

const Configure = (props) => {
    return (
    <div>
        <table className='table'>
            <thead>
                <tr>
                    <th>User</th>
                    <th>Calendar</th>
                    <th>Timer</th>
                    <th>Show summary</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td><h5>{props.configurations.user}</h5></td>
                    <td><Calendar /></td>
                    <td><Timer /></td>
                    <td><Summary /></td>
                </tr>
            </tbody>
        </table>
        <button onClick={() => {
            props.sendConfigurations(props.configurations)}}>Send</button>
    </div>
    )
}

export default Configure;