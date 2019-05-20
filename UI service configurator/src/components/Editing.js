import React from 'react';
import User from '../containers/userContainer';
import Credentials from '../containers/credentialsContainer';

const Editing = () => {
    return (
        <div>
            <table className='table'>
                <thead>
                    <tr>
                        <th>User</th>
                        <th>Credentials</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td><User /></td>
                        <td><Credentials /></td>
                    </tr>
                </tbody>
            </table>
        </div>
    )
}

export default Editing;