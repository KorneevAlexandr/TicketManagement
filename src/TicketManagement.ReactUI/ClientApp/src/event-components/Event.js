import React from 'react';
import { useParams } from "react-router-dom";

import EventAreas from './EventAreas';

function Event() {

    const { id } = useParams();

    return (
        <div>
            <EventAreas eventId={id} />
        </div>
    );
}

export default Event;