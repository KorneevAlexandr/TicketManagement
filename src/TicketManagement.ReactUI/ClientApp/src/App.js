import './App.css';
import { TicketHeader } from './TicketHeader';
import { EventList } from './event-components/EventList';
import Event from './event-components/Event';
import { User } from './user-components/User';
import { BrowserRouter, Route, Routes, NavLink } from 'react-router-dom';
import { TicketFooter } from './TicketFooter';

function App() {
    return (
        <BrowserRouter>
            <TicketHeader />
            <div className="App container">

                <Routes>
                    <Route path="/eventList" element={<EventList />} />
                    <Route path="/event/:id" element={<Event />} />
                    <Route path="/user" element={<User />} />
                </Routes>

            </div>

            <TicketFooter />
        </BrowserRouter>
    )
}


export default App;