import React from 'react';


const Calendar = ({ items , currentState}) => (
     <div>
        {
            items.map((item, i) =>
            {
                return <List key={i} name = {item.name} items={currentState==="present"?item.present:item.previous} />
            })
        } 
    </div>
);


const List = ({ items , name}) => (
    <div className="calendar">
        <div className = "name">{ name }</div>            
            <div>
                {
                    items.map((item, i) =>
                    {
                        return <Appointment key={i} item={item}/>
                    })
                }
            </div>
    </div>
);

const Appointment = ({ item }) => (
        <div className="appointment">
            <div className = "subject">{item.subject}</div>
            <div className = "description">{item.description}</div>
            <div className = "date">{item.date}</div>
            <div className = "attendees">{item.attendees}</div>
        </div>
);


const History = (props) => {
    return (
        <div>
            <Calendar items={props.calendars} currentState = {props.currentState} />
            <button onClick={() => {
                    props.changeHistoryState();
                }}>
                    {props.currentState==="present"?"Previous":"Present"}
            </button>
        </div>
    )
}


export default History;

