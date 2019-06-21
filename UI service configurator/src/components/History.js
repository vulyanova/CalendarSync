import React, {useState} from 'react';
import useInfiniteScroll from '../useInfiniteScroll';

const Events = ({ events}) => (
     <tbody>
        {
            events.map((event, i) =>
            {
                return <Event key={i} value = {event._id} event={event} />
            })
        } 
    </tbody>
);

const Event = ({ event , value}) => {
    return (
    <tr >
        <td >{event.user} </td>  
        <td >{event.time} </td> 
        <td >{event.calendar} </td> 
        <td >{event.action} </td> 
        <State item = {event.previousState}/> 
        <State item = {event.presentState}/> 
    </tr>
    )
};

const State = ({ item }) => (
    <td className="state" >
         <span>{item.subject}</span>
         <span>{item.description}</span>
         <span>{item.location}</span>
         <span>{item.attendees}</span>
         <span>{item.date}</span>
    </td>
);

const History = (props) => {
    const [page, incPage] = useState(2);
    const [isFetching, setIsFetching] = useInfiniteScroll(fetchMoreListItems);
    const size = 20;

    function fetchMoreListItems() {
        props.addHistoryState(size, page);
        incPage(page + 1);
        setIsFetching(false);   
    }
    
    return (
        <div>
            <table className = "logs">
                <thead>
                    <tr>
                        <th>User</th>
                        <th>Updated</th>
                        <th>Calendar</th>
                        <th>Action</th>
                        <th>Previous state</th>
                        <th>Current state</th>
                    </tr>
                </thead>
                <Events events={props.calendars} />               
            </table>
            {isFetching && 'Fetching logs...'}
        </div>
    )
}

export default History;

