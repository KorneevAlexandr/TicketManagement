import { React, Component } from 'react';
import { Variables } from '../Variables';
import userData from '../localization/userData'
import LocalizedStrings from 'react-localization';
import { read_cookie } from "sfcookies";

let strings = new LocalizedStrings(userData);

export class SettingsModal extends Component {

    constructor(props) {
        super(props)

        this.maxName = 30;
        this.maxSurname = 40;
        this.minAge = 3;
        this.maxAge = 120;
        this.maxEmail = 40;
        this.maxLogin = 60;
        this.maxPassword = 60;

        this.state = {
            language: read_cookie("language"),
            user: {},
        }
    }

    refreshUser() {
        fetch(Variables.API_URL + "user/userSettings",
            { method: "GET", headers: { "Content-Type": "application/json" } })
            .then(response => response.json())
            .then(data => {
                this.setState({ user: data })
            });
    }

    componentDidMount() {
        this.refreshUser();
    }

    changeName = (e) => {
        if (e.target.value.length <= this.maxName) {
            this.setState(prevState => {
                let user = Object.assign({}, prevState.user);
                user.name = e.target.value;
                return { user };
            })
        }
    }
    changeSurname = (e) => {
        if (e.target.value.length <= this.maxSurname) {
            this.setState(prevState => {
                let user = Object.assign({}, prevState.user);
                user.surname = e.target.value;
                return { user };
            })
        }
    }
    changeEmail = (e) => {
        if (e.target.value.length <= this.maxEmail) {
            this.setState(prevState => {
                let user = Object.assign({}, prevState.user);
                user.email = e.target.value;
                return { user };
            })
        }
    }
    changeAge = (e) => {
        if (e.target.value <= this.maxAge) {
            this.setState(prevState => {
                let user = Object.assign({}, prevState.user);
                user.age = e.target.value;
                return { user };
            })
        }
    }
    changeLogin = (e) => {
        if (e.target.value.length <= this.maxLogin) {
            this.setState(prevState => {
                let user = Object.assign({}, prevState.user);
                user.login = e.target.value;
                return { user };
            })
        }
    }
    changePassword = (e) => {
        if (e.target.value.length <= this.maxPassword) {
            this.setState(prevState => {
                let user = Object.assign({}, prevState.user);
                user.password = e.target.value;
                return { user };
            })
        }
    }

    validModel() {
        const minLength = 0;
        const dangerColor = "red";
        const successColor = "green";
        let result = true;

        const nameInput = document.getElementById("nameInput");
        const surnameInput = document.getElementById("surnameInput");
        const ageInput = document.getElementById("ageInput");
        const emailInput = document.getElementById("emailInput");
        const loginInput = document.getElementById("loginInput");
        const passwordInput = document.getElementById("passwordInput");

        if (nameInput.value.length == minLength) {
            nameInput.style.borderColor = dangerColor;
            result = false;
        } else {
            nameInput.style.borderColor = successColor;
        }
        if (surnameInput.value.length == minLength) {
            surnameInput.style.borderColor = dangerColor;
            result = false;
        } else {
            surnameInput.style.borderColor = successColor;
        }
        if (ageInput.value < this.minAge || ageInput.value >= this.maxAge) {
            ageInput.style.borderColor = dangerColor;
            result = false;
        } else {
            ageInput.style.borderColor = successColor;
        }
        if (emailInput.value.length == minLength) {
            emailInput.style.borderColor = dangerColor;
            result = false;
        } else {
            emailInput.style.borderColor = successColor;
        }
        if (loginInput.value.length == minLength) {
            loginInput.style.borderColor = dangerColor;
            result = false;
        } else {
            loginInput.style.borderColor = successColor;
        }
        if (passwordInput.value.length == minLength) {
            passwordInput.style.borderColor = dangerColor;
            result = false;
        } else {
            passwordInput.style.borderColor = successColor;
        }

        return result;
    }

    updateSettings() {
        if (this.validModel()) {
            fetch(Variables.API_URL + "user/userSettings", {
                method: 'PUT',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    id: this.state.user.id,
                    name: this.state.user.name,
                    surname: this.state.user.surname,
                    email: this.state.user.email,
                    age: this.state.user.age,
                    login: this.state.user.login,
                    password: this.state.user.password
                })
            })
                .then(res => res.json())
                .then((result) => {
                    alert(strings.successReload);
                    fetch(Variables.API_URL + "user/exit",
                        { method: "POST" });
                    window.location.assign(Variables.MVC_URL + "Account/Exit");
                }, (error) => {
                    alert(strings.fail);
                })
        }
    }

    render() {
        strings.setLanguage(this.state.language);
        const {
            user,
        } = this.state;

        return (
            <div className="modal-dialog modal-lg modal-dialog-centered">
                <div class="modal-content">
                    <div className="modal-header">
                        <h5 className="modal-title text-dark">{strings.updateUserSettings}</h5>
                        <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>

                    <div className="modal-body">

                        <div className="input-group mb-3">
                            <span className="input-group-text">{strings.name}</span>
                            <input type="text" id="nameInput" className="form-control"
                                onChange={this.changeName}
                                value={user.name} />
                        </div>
                        <div className="input-group mb-3">
                            <span className="input-group-text">{strings.surname}</span>
                            <input type="text" id="surnameInput" className="form-control"
                                onChange={this.changeSurname}
                                value={user.surname} />
                        </div>
                        <div className="input-group mb-3">
                            <span className="input-group-text">{strings.email}</span>
                            <input type="email" id="emailInput" className="form-control"
                                onChange={this.changeEmail}
                                value={user.email} />
                        </div>
                        <div className="input-group mb-3">
                            <span className="input-group-text">{strings.age}</span>
                            <input type="number" id="ageInput" className="form-control"
                                onChange={this.changeAge}
                                value={user.age} />
                        </div>
                        <div className="input-group mb-3">
                            <span className="input-group-text">{strings.login}</span>
                            <input type="text" id="loginInput" className="form-control"
                                onChange={this.changeLogin}
                                value={user.login} />
                        </div>
                        <div className="input-group mb-3">
                            <span className="input-group-text">{strings.password}</span>
                            <input type="text" id="passwordInput" className="form-control"
                                onChange={this.changePassword}
                                value={user.password} />
                        </div>

                        <button type="button"
                            className="btn btn-primary float-start"
                            onClick={() => this.updateSettings()}>
                            {strings.updateSettings}</button>

                    </div>

                </div>
            </div>
        )
    }
}
