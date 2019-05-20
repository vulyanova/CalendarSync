import React, {Component} from 'react';

class CurrentState extends Component{
    render(){
        return(
            <div>
                <table className='table'>
                    <thead>
                        <tr>
                            <th>User</th>
                            <th>Timer (min)</th>
                            <th>Summary</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>{this.props.user}</td>
                            <td>{this.props.timer}</td>
                            <td>{this.props.showSummary ? "Show" : "Hide"} summary</td>
                        </tr>
                    </tbody>
                </table>
        </div> 
        )}
}

export default CurrentState;