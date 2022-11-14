import { React, Component } from 'react';
import { NavLink } from 'react-router-dom';
import { Variables } from '../Variables';
import { SettingsModal } from './SettingsModal'
import userData from '../localization/userData'
import LocalizedStrings from 'react-localization';
import { bake_cookie, read_cookie } from "sfcookies";
import './user.css'

let strings = new LocalizedStrings(userData);

export class User extends Component {

    constructor(props) {
        super(props)

        let lang = read_cookie("language");

        this.minScore = -1;
        this.maxScore = 100000;

        this.state = {
            language: lang == null ? "en" : lang,
            user: {},
            tickets: [],
            score: 0,
        }

        document.title = strings.titleName;

        this.handleLanguageChange = this.handleLanguageChange.bind(this);
    }

    handleLanguageChange(e) {
        e.preventDefault();
        let lang = e.target.value;
        bake_cookie("language", lang);
        this.setState(prevState => ({
            language: lang
        }));

        window.location.reload();
    }

    refreshUser() {
        fetch(Variables.API_URL + "user/user",
            { method: "GET", headers: { "Content-Type": "application/json" } })
            .then(response => response.json())
            .then(data => {
                this.setState({ user: data })
            });

        fetch(Variables.API_URL + "user/userTickets",
            { method: "GET", headers: { "Content-Type": "application/json" } })
            .then(response => response.json())
            .then(data => {
                this.setState({ tickets: data })
            });
    }
     
    componentDidMount() {
        this.refreshUser();
    }

    exit() {
        if (window.confirm(strings.areYouSure)) {
            fetch(Variables.API_URL + "user/exit",
                { method: "POST" });

            window.location.assign(Variables.MVC_URL + "Account/Exit");
        }
    }

    changeScore = (e) => {
        if (e.target.value > this.minScore && e.target.value < this.maxScore) {
            this.setState({
                score: e.target.value
            });
        }
    }

    upBalance() {
        if (this.state.score > this.minScore && this.state.score < this.maxScore) {
            fetch(Variables.API_URL + "user/userBalance", {
                method: 'PUT',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    id: this.state.user.id,
                    score: this.state.score
                })
            })
                .then(res => res.json())
                .then((result) => {
                    alert(strings.success);
                    this.refreshUser();
                }, (error) => {
                    alert(strings.fail);
                })
        }
    }

    render() {
        strings.setLanguage(this.state.language);
        const {
            user,
            tickets,
            score,
        } = this.state;

		return (
			<div className="wrapper">
                <div className="user-info">
                    <h2 className="text-dark">{strings.infoAboutUser}</h2>

                    <div className="action-block">

                        <button type="button"
                            className="ref-action up-score-action"
                            data-bs-toggle="modal"
                            data-bs-target="#balanceModal">
                            {strings.topUpBalance}
                        </button>
                        <button type="button"
                            className="ref-action up-user-action"
                            data-bs-toggle="modal"
                            data-bs-target="#settingsModal">
                            {strings.updateSettings}
                        </button>

                        {user.roleName === "ModeratorEvent" ?
                            <NavLink className="ref-action management-action" to="/eventList">{strings.eventManagement}</NavLink>
                            : null
                        }
                        {user.roleName === "ModeratorEvent" ?
                            <a className="ref-action management-action" href={Variables.MVC_URL + "ThirdPartyEvent"}>{strings.thirdPartyEventManagement}</a>
                            : null
                        }
                        {user.roleName === "ModeratorVenue" ?
                            <a className="ref-action management-action" href={Variables.MVC_URL + "ModeratorVenue"}>{strings.venueManagement}</a>
                            : null
                        }
                        <button className="ref-action exit-action" onClick={() => this.exit()}>{strings.exit}</button>

                    </div>

                    <div className="user-info-block">
                        {strings.lang} <select onChange={this.handleLanguageChange}>
                        <option>Any</option>
                        <option value="en">English</option>
                        <option value="ru">Russian</option>
                        <option value="be">Belarussian</option>
                        </select>
                    </div>
                    <div className="user-info-block">
                        <div className="permanent-user-block">{strings.name}: </div>
                        <div className="user-value-block">{ user.name }</div>
				    </div>
				    <div className="user-info-block">
                        <div className="permanent-user-block">{strings.surname}</div>
                        <div className="user-value-block">{ user.surname }</div>
				    </div>
                    <div className="user-info-block">
                        <div className="permanent-user-block">{strings.email}</div>
                        <div className="user-value-block">{ user.email }</div>
				    </div>
                    <div className="user-info-block">
                        <div className="permanent-user-block">{strings.age}</div>
                        <div className="user-value-block">{ user.age }</div>
				    </div>
                    <div className="user-info-block">
                        <div className="permanent-user-block">{strings.login}</div>
                        <div className="user-value-block">{user.login}</div>
				    </div>
                    <div className="user-info-block">
                        <div className="permanent-user-block">{strings.score}</div>
                        <div className="user-value-block">{ user.score }</div>
				    </div>
                </div>

                <h2 className="h2-ticket">{strings.boughtTickets}</h2>
                <table style={{ marginBottom: 10 }} className="table table-striped">
                    <thead>
                        <tr>
                            <th>{strings.ticketDate}</th>
                            <th>{strings.ticketEventName}</th>
                            <th>{strings.ticketPrice}, $</th>
                        </tr>
                    </thead>

                    <tbody>
                        {tickets.map(ticket =>
                            <tr>
                                <td>{ticket.dateTimePurchase}</td>
                                <td>{ticket.eventName}</td>
                                <td>{ticket.price}</td>
                            </tr>
                        )}
                    </tbody>
                </table>

                <div className="modal fade" id="balanceModal" tabIndex="-1" aria-hidden="true">
                    <div className="modal-dialog modal-lg modal-dialog-centered">
                        <div class="modal-content">
                            <div className="modal-header">
                                <h5 className="modal-title text-dark">{strings.nowBalance}: {user.score}</h5>
                                <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                            </div>

                            <div className="modal-body">

                                <div className="input-group mb-3">
                                    <span className="input-group-text">{strings.enterSum}</span>
                                    <input type="number" id="changeBalanceInput" className="form-control"
                                        value={score}
                                        onChange={this.changeScore} />
                                </div>

                                <button type="button"
                                    className="btn btn-primary float-start"
                                    onClick={() => this.upBalance()}>
                                    {strings.topUp}</button>

                            </div>

                        </div>
                    </div>
                </div>

                <div className="modal fade" id="settingsModal" tabIndex="-1" aria-hidden="true">
                    <SettingsModal />
                </div>
            </div>
            );
    }
}
