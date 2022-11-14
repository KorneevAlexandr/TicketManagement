import React, { Component } from "react";
import { Variables } from '../Variables'
import eventAreasData from '../localization/eventAreasData'
import LocalizedStrings from 'react-localization';
import { read_cookie } from "sfcookies";

let strings = new LocalizedStrings(eventAreasData);

class EventAreas extends Component {

	constructor(props) {
		super(props);

		this.maxDescription = 200;
		this.maxX = 10000;
		this.maxY = 10000;
		this.maxPrice = 10000;

		const eventId = this.props.eventId;

		this.state = {
			language: read_cookie("language"),
			id: eventId,
			selectedEvent: {},
			eventAreas: [],
			seats: [],
			eventAreaId: 0,
			description: "",
			coordX: 0,
			coordY: 0,
			price: 0,		
		}
	}

	refreshAreas() {
		fetch(Variables.API_URL + "eventManagement/Event/" + this.state.id,
			{ method: "GET", headers: { "Content-Type": "application/json" } })
			.then(response => response.json())
			.then(data => {
				this.setState({ selectedEvent: data })
			});

		fetch(Variables.API_URL + "eventAreaManagement/EventAreas/" + this.state.id,
			{ method: "GET", headers: { "Content-Type": "application/json" } })
			.then(response => response.json())
			.then(data => {
				this.setState({ eventAreas: data })
			});
	}

	componentDidMount() {
		this.refreshAreas();
	}

	changeDescription = (e) => {
		if (e.target.value.length <= this.maxDescription) {
			this.setState({ description: e.target.value });
		}
	}
	changeCoordX = (e) => {
		if (e.target.value <= this.maxX) {
			this.setState({ coordX: e.target.value });
		}
	}
	changeCoordY = (e) => {
		if (e.target.value <= this.maxY) {
			this.setState({ coordY: e.target.value });
		}
	}
	changePrice = (e) => {
		if (e.target.value <= this.maxPrice) {
			this.setState({ price: e.target.value });
		}
	}

	validModel() {
		const minLength = 0;
		const dangerColor = "red";
		const successColor = "green";
		let result = true;

		const descriptionInput = document.getElementById("descriptionInput");
		const xInput = document.getElementById("xInput");
		const yInput = document.getElementById("yInput");
		const priceInput = document.getElementById("priceInput");

		if (descriptionInput.value.length == minLength) {
			descriptionInput.style.borderColor = dangerColor;
			result = false;
		} else {
			descriptionInput.style.borderColor = successColor;
		}
		if (xInput.value <= 0 || xInput.value > this.maxX) {
			xInput.style.borderColor = dangerColor;
			result = false;
		} else {
			xInput.style.borderColor = successColor;
		}
		if (yInput.value <= 0 || yInput.value > this.maxY) {
			yInput.style.borderColor = dangerColor;
			result = false;
		} else {
			yInput.style.borderColor = successColor;
		}
		if (priceInput.value <= 0 || priceInput.value > this.maxPrice) {
			priceInput.style.borderColor = dangerColor;
			result = false;
		} else {
			priceInput.style.borderColor = successColor;
		}

		return result;
	}

	editClick(e) {
		this.setState({
			eventAreaId: e.id,
			description: e.description,
			price: e.price,
			coordX: e.coordX,
			coordY: e.coordY,
		});
	}

	eventSeatsClick(id) {
		fetch(Variables.API_URL + "eventAreaManagement/EventSeats/" + id,
			{ method: "GET", headers: { "Content-Type": "application/json" } })
			.then(response => response.json())
			.then(data => {
				this.setState({ seats: data });
			});
	}

	deleteEventArea(id) {
		if (window.confirm(strings.areYouSure)) {
			fetch(Variables.API_URL + "eventAreaManagement/EventArea/" + id, {
				method: 'DELETE',
				headers: {
					'Accept': 'application/json',
					'Content-Type': 'application/json'
				}
			})
				.then(res => res.json())
				.then((result) => {
					alert(strings.success);
					this.refreshAreas();
				}, (error) => {
					alert(strings.fail);
				})
		}
	}

	editEventArea() {
		if (this.validModel()) {
			fetch(Variables.API_URL + "eventAreaManagement/EventArea", {
				method: 'PUT',
				headers: {
					'Accept': 'application/json',
					'Content-Type': 'application/json'
				},
				body: JSON.stringify({
					id: this.state.eventAreaId,
					price: this.state.price,
					description: this.state.description,
					coordX: this.state.coordX,
					coordY: this.state.coordY,
				})
			})
				.then(res => res.json())
				.then((result) => {
					this.refreshAreas();
				}, (error) => {
					alert(strings.fail);
				})
		}
	}

	changeSeatState(id, eventAreaId) {
		fetch(Variables.API_URL + "eventAreaManagement/EventSeat/" + id, {
			method: 'PUT',
			headers: {
				'Accept': 'application/json',
				'Content-Type': 'application/json'
			}
		});
		this.eventSeatsClick(eventAreaId);
	}

	render() {
		strings.setLanguage(this.state.language);
		const {
			selectedEvent,
			eventAreas,
			seats,
			description,
			coordX,
			coordY,
			price
		} = this.state;

		return (
			<div>
				<h2 className="mt-2">{strings.eventName}: {selectedEvent.name}</h2>
				<p>{strings.timeStart}: {selectedEvent.dateTimeStart}</p>
				<p>{strings.timeEnd}: {selectedEvent.dateTimeEnd}</p>
				<img width="160" height="200" src={selectedEvent.url} />
				<br /><br />
				<h3>{strings.eventAreas}</h3>

				<table className="table table-striped">
					<thead>
						<tr>
							<th>{strings.description}</th>
							<th>{strings.coordX}</th>
							<th>{strings.coordY}</th>
							<th>{strings.price}</th>
							<th>{strings.options}</th>
						</tr>
					</thead>
					<tbody>
						{eventAreas.map(eventArea =>
							<tr>
								<td>{ eventArea.description }</td>
								<td>{eventArea.coordX}</td>
								<td>{eventArea.coordY}</td>
								<td>{eventArea.price}</td>
								<td>
									<button type="button"
										className="btn btn-light mr-1"
										data-bs-toggle="modal"
										data-bs-target="#eventAreaEditModal"
										onClick={() => this.editClick(eventArea)}>
										{strings.edit}
									</button>
									<button type="button"
										className="btn btn-light mr-1"
										data-bs-toggle="modal"
										data-bs-target="#eventSeatsMapModal"
										onClick={() => this.eventSeatsClick(eventArea.id)}>
										{strings.eventSeats}
									</button>
									<button type="button"
										className="btn btn-light mr-1"
										data-bs-toggle="modal"
										onClick={() => this.deleteEventArea(eventArea.id)}>
										{strings.delete}
									</button>
								</td>
							</tr>
							)}
					</tbody>
				</table>

				<div className="modal fade" id="eventAreaEditModal" tabIndex="-1" aria-hidden="true">
					<div className="modal-dialog modal-lg modal-dialog-centered">
						<div class="modal-content">
							<div className="modal-header">
								<h5 className="modal-title text-dark">{strings.editTitle}</h5>
								<button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
							</div>

							<div className="modal-body">

								<div className="input-group mb-3">
									<span className="input-group-text">{strings.description}</span>
									<input type="text" id="descriptionInput" className="form-control"
										value={description}
										onChange={this.changeDescription} />
								</div>
								<div className="input-group mb-3">
									<span className="input-group-text">{strings.coordX}</span>
									<input type="number" id="xInput" className="form-control"
										value={coordX}
										onChange={this.changeCoordX} />
								</div>
								<div className="input-group mb-3">
									<span className="input-group-text">{strings.coordY}</span>
									<input type="number" id="yInput" className="form-control"
										value={coordY}
										onChange={this.changeCoordY} />
								</div>
								<div className="input-group mb-3">
									<span className="input-group-text">{strings.price}</span>
									<input type="number" id="priceInput" className="form-control"
										value={price}
										onChange={this.changePrice} />
								</div>

								<button type="button"
									className="btn btn-primary float-start"
									onClick={() => this.editEventArea()}>
									{strings.edit}</button>

							</div>

						</div>
					</div>
				</div>

				<div className="modal fade" id="eventSeatsMapModal" tabIndex="-1" aria-hidden="true">
					<div className="modal-dialog modal-lg modal-dialog-centered">
						<div class="modal-content">
							<div className="modal-header">
								<h5 className="modal-title text-dark">{strings.eventSeatsMap}</h5>
								<button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
							</div>

							<div className="modal-body">
								{seats.map(seat =>
									<div>
										{seat.booked == false ?
											<button type="button"
												className="btn bg-success mb-1"
												title={strings.delete}
												onClick={() => this.changeSeatState(seat.id, seat.eventAreaId)}>
												S</button> :
											<button type="button"
												className="btn bg-secondary mb-1"
												title={strings.deleted}>
												S</button>
										}
									</div>
									)}
							</div>

						</div>
					</div>
				</div>

			</div>
		)
	}	
}

export default EventAreas;