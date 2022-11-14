import React, {Component} from "react";
import { NavLink } from 'react-router-dom';
import { Variables } from '../Variables'
import eventListData from '../localization/eventListData'
import LocalizedStrings from 'react-localization';
import { read_cookie } from "sfcookies";

let strings = new LocalizedStrings(eventListData);

export class EventList extends Component
{
    constructor(props) {
        super(props);

        this.maxName = 120;
        this.maxPrice = 1000000;
        this.maxUrl = 2000;

        this.state = {
            language: read_cookie("language"),
            events: [],
            venues: [],
            layouts: [],
            venueId: 0,
            layoutId: 0,
            id: 0,
            name: "",
            description: "",
            url: "",
            dateTimeStart: "",
            dateTimeEnd: "",
            price: 0
        }
    }

    refreshList() {
        fetch(Variables.API_URL + "venueManagement/Venues",
            { method: "GET", headers: { "Content-Type": "application/json" } })
            .then(response => response.json())
            .then(data => {
                this.setState({ venues: data })
            });
    }

    componentDidMount() {
        this.refreshList();
    }

    changeGlobalVenueId = (e) => {
        fetch(Variables.API_URL + "eventManagement/Events/" + e.target.value,
            { method: "GET", headers: { "Content-Type": "application/json" } })
            .then(response => response.json())
            .then(data => {
                this.setState({ events: data })
            });
    }

    changeName = (e) => {
        if (e.target.value.length <= this.maxName) {
            this.setState({
                name: e.target.value
            });
        }
    }
    changeDescription = (e) => {
        this.setState({
            description: e.target.value
        });
    }
    changeStartDate = (e) => {
        this.setState({
            dateTimeStart: e.target.value
        });
    }
    changeEndDate = (e) => {
        this.setState({
            dateTimeEnd: e.target.value
        });
    }
    changePrice = (e) => {
        if (e.target.value <= this.maxPrice || e.target.value > -1) {
            this.setState({
                price: e.target.value
            });
        }
    }
    changeUrl = (e) => {
        if (e.target.value.length <= this.maxUrl) {
            this.setState({
                url: e.target.value
            });
        }
    }
    changeVenueId = (e) => {
        fetch(Variables.API_URL + "venueManagement/Layouts?venueId=" + e.target.value,
            {
                method: "GET", headers: { "Content-Type": "application/json" }
            })
            .then(response => response.json())
            .then(data => {
                this.setState({ layouts: data })
            });
        this.setState({
            venueId: e.target.value,
        });
    }
    changeLayoutId = (e) => {
        this.setState({
            layoutId: e.target.value
        });
    }

    validModel() {
        const minLength = 0;
        const dangerColor = "red";
        const successColor = "green";
        let result = true;

        const layoutSelect = document.getElementById("layoutSelect");
        const nameInput = document.getElementById("nameInput");
        const descriptionInput = document.getElementById("descriptionInput");
        const dateStartInput = document.getElementById("dateStartInput");
        const dateEndInput = document.getElementById("dateEndInput");
        const urlInput = document.getElementById("urlInput");

        if (this.state.layoutId == 0) {
            layoutSelect.style.borderColor = dangerColor;
            result = false;
        } else {
            layoutSelect.style.borderColor = successColor;
        }

        if (nameInput.value.length == minLength) {
            nameInput.style.borderColor = dangerColor;
            result = false;
        } else {
            nameInput.style.borderColor = successColor;
        }
        if (descriptionInput.value.length == minLength) {
            descriptionInput.style.borderColor = dangerColor;
            result = false;
        } else {
            descriptionInput.style.borderColor = successColor;
        }
        if (dateStartInput.value.length == minLength) {
            dateStartInput.style.borderColor = dangerColor;
            result = false;
        } else {
            dateStartInput.style.borderColor = successColor;
        }
        if (dateEndInput.value.length == minLength) {
            dateEndInput.style.borderColor = dangerColor;
            result = false;
        } else {
            dateEndInput.style.borderColor = successColor;
        }
        if (urlInput.value.length == minLength) {
            urlInput.style.borderColor = dangerColor;
            result = false;
        } else {
            urlInput.style.borderColor = successColor;
        }

        return result;
    }

    addClick() {
        var venue = this.state.venues[0];
        fetch(Variables.API_URL + "venueManagement/Layouts?venueId=" + venue.id,
            {
                method: "GET", headers: { "Content-Type": "application/json" }
            })
            .then(response => response.json())
            .then(data => {
                this.setState({
                    layouts: data,
                    layoutId: data[0].id
                })
            });

        this.setState({
            id: 0,
            name: "",
            description: "",
            url: "",
            dateTimeStart: "",
            dateTimeEnd: "",
            price: 0,
            venueId: 0,
            layoutId: 0,
        });
    }

    editClick(e) {
        fetch(Variables.API_URL + "venueManagement/Layouts?venueId=" + e.venueId,
            {
                method: "GET", headers: { "Content-Type": "application/json" }
            })
            .then(response => response.json())
            .then(data => {
                this.setState({ layouts: data })
            });

        this.setState({
            id: e.id,
            name: e.name,
            description: e.description,
            price: e.price,
            url: e.url,
            layoutId: e.layoutId,
            dateTimeStart: e.dateTimeStart,
            dateTimeEnd: e.dateTimeEnd
        });
    }

    createEvent() {
        if (this.validModel()) {
            fetch(Variables.API_URL + "eventManagement/Event", {
                method: 'POST',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    name: this.state.name,
                    description: this.state.description,
                    dateTimeStart: this.state.dateTimeStart,
                    dateTimeEnd: this.state.dateTimeEnd,
                    venueId: this.state.venueId,
                    layoutId: this.state.layoutId,
                    price: this.state.price,
                    url: this.state.url
                })
            })
                .then(res => res.json())
                .then((result) => {
                    alert(strings.success);
                    this.refreshList();
                }, (error) => {
                    alert(strings.failDate);
                });
            this.componentDidMount();
        }
    }

    editEvent() {
        if (this.validModel()) {
            fetch(Variables.API_URL + "eventManagement/Event", {
                method: 'PUT',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    id: this.state.id,
                    name: this.state.name,
                    description: this.state.description,
                    dateTimeStart: this.state.dateTimeStart,
                    dateTimeEnd: this.state.dateTimeEnd,
                    layoutId: this.state.layoutId,
                    url: this.state.url
                })
            })
                .then(res => res.json())
                .then((result) => {
                    alert(strings.success);
                    this.refreshList();
                }, (error) => {
                    alert(strings.failDate);
                });
            this.componentDidMount();
        }
    }

    deleteEvent(id) {
        if (window.confirm(strings.areYouSure)) {
            fetch(Variables.API_URL + "eventManagement/Event/" + id, {
                method: 'DELETE',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                }
            })
                .then(res => res.json())
                .then((result) => {
                    alert(strings.success);
                    this.refreshList();
                }, (error) => {
                    alert(strings.fail);
                });
            this.componentDidMount();
        }

    }

    render() {
        strings.setLanguage(this.state.language);
        const {
            events,
            venues,
            layouts,
            id,
            name,
            description,
            dateTimeStart,
            dateTimeEnd,
            price,
            url
        } = this.state;

        return (
            <div>

                <div className="input-group mb-3 w-50 p-2">
                    <span className="input-group-text">{strings.venueName}</span>
                    <select className="form-select"
                        onChange={this.changeGlobalVenueId}>
                        {venues.map(venue => <option value={venue.id} key={venue.id}>{venue.name}</option>)}
                    </select>
                </div>

                <button type="button"
                    className="btn btn-primary m-2 float-end"
                    data-bs-toggle="modal"
                    data-bs-target="#exampleModal"
                    onClick={() => this.addClick()}>
                    {strings.createEvent}
                </button>

                <table className="table table-striped">
                    <thead>
                        <tr>
                            <th>{strings.name}</th>
                            <th>{strings.description}</th>
                            <th>{strings.timeStart}</th>
                            <th>{strings.timeEnd}</th>
                            <th>{strings.image}</th>
                            <th>{strings.option}</th>
                        </tr>
                    </thead>
                    <tbody>
                        {events.map(event=>
                            <tr key={event.id}>
                                <td style={{ display: "table-cell", verticalAlign: "middle" }}>{event.name}</td>
                                <td style={{ display: "table-cell", verticalAlign: "middle" }}>{event.description.substring(0, 30)+"..."}</td>
                                <td style={{ display: "table-cell", verticalAlign: "middle" }}>{event.dateTimeStart}</td>
                                <td style={{ display: "table-cell", verticalAlign: "middle" }}>{event.dateTimeEnd}</td>
                                <td style={{ display: "table-cell", verticalAlign: "middle" }}>
                                    <img width="100" height="120" src={event.url} />
                                </td>
                                <td>
                                    
                                    <NavLink className="btn btn-light mr-1"
                                        to={`/event/${event.id}`}>{strings.details}</NavLink>

                                    <button type="button"
                                        className="btn btn-light mr-1"
                                        data-bs-toggle="modal"
                                        data-bs-target="#exampleModal"
                                        onClick={() => this.editClick(event)}>
                                        {strings.edit}
                                    </button>

                                    <button type="button"
                                        className="btn btn-light mr-1"
                                        data-bs-toggle="modal"
                                        onClick={() => this.deleteEvent(event.id)}>
                                        {strings.delete}
                                    </button>
                                </td>
                            </tr>
                        )}
                    </tbody>
                </table>

                <div className="modal fade" id="exampleModal" tabIndex="-1" aria-hidden="true">
                    <div className="modal-dialog modal-lg modal-dialog-centered">
                        <div class="modal-content">
                            <div className="modal-header">
                                <h5 className="modal-title">
                                    {id == 0 ? strings.createTitle : strings.editTitle }
                                </h5>
                                <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                            </div>

                            <div className="modal-body">

                                {id == 0 ?
                                    <div className="input-group mb-3">
                                        <span className="input-group-text">{strings.venueName}</span>
                                        <select id="venueSelect" className="form-select"
                                            onChange={this.changeVenueId}>
                                            {venues.map(venue => <option value={venue.id} key={venue.id}>{venue.name}</option>)}
                                        </select>
                                        <span className="input-group-text">{strings.layoutName}</span>
                                        <select id="layoutSelect" className="form-select"
                                            onChange={this.changeLayoutId}>
                                            {layouts.map(layout => <option value={layout.id} key={layout.id}>{layout.name}</option>)}
                                        </select>
                                    </div> : null}

                                {id != 0 ?
                                    <div className="input-group mb-3">
                                        <span className="input-group-text">{strings.layoutName}</span>
                                        <select id="layoutSelect" className="form-select"
                                            onChange={this.changeLayoutId}>
                                            {layouts.map(layout => <option value={layout.id} key={layout.id}>{layout.name}</option>)}
                                        </select>
                                    </div> : null}

                                <div className="input-group mb-3">
                                    <span className="input-group-text">{strings.name}</span>
                                    <input type="text" id="nameInput" className="form-control"
                                        value={name}
                                        onChange={this.changeName} />
                                </div>
                                <div className="input-group mb-3">
                                    <span className="input-group-text">{strings.description}</span>
                                    <input type="text" id="descriptionInput" className="form-control"
                                        value={description}
                                        onChange={this.changeDescription} />
                                </div>
                                <div className="input-group mb-3">
                                    <span className="input-group-text">{strings.timeStart}</span>
                                    <input type="datetime-local" id="dateStartInput" className="form-control"
                                        value={id != 0 ? dateTimeStart: null}
                                        onChange={this.changeStartDate} />

                                    <span className="input-group-text">{strings.timeEnd}</span>
                                    <input type="datetime-local" id="dateEndInput" className="form-control"
                                        value={id != 0 ? dateTimeEnd: null}
                                        onChange={this.changeEndDate} />
                                </div>

                                {id == 0 ? 
                                    <div className="input-group mb-3">
                                        <span className="input-group-text">{strings.price}</span>
                                        <input type="number" id="priceInput" className="form-control"
                                            value={price}
                                            onChange={this.changePrice} />
                                    </div>: null}

                                <div className="input-group mb-3">
                                    <span className="input-group-text">{strings.imageUrl}</span>
                                    <input type="url" id="urlInput" className="form-control"
                                        value={url}
                                        onChange={this.changeUrl} />
                                </div>

                                {id == 0 ?
                                    <button type="button"
                                        className="btn btn-primary float-start"
                                        onClick={() => this.createEvent()}>
                                        {strings.create}</button>
                                    :null}

                                {id != 0 ?
                                    <button type="button"
                                        className="btn btn-primary float-start"
                                        onClick={() => this.editEvent()}>
                                        {strings.edit}</button>
                                    : null}

                            </div>

                        </div>
                    </div>
                </div>

            </div>
        )
    }
}

