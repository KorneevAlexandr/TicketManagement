import React, {Component} from "react";
import { NavLink } from "react-router-dom";
import { Variables } from "./Variables";
import './header-footer.css'
import headerData from './localization/headerData';
import LocalizedStrings from 'react-localization';
import { read_cookie } from "sfcookies";

let strings = new LocalizedStrings(headerData);

export class TicketHeader extends Component
{
    constructor(props) {
		super(props);

		this.state = {
			language: read_cookie("language"),
			name: "",
		}
    }

	refreshUsername() {
		fetch(Variables.API_URL + "user/username",
			{ method: "GET", headers: { "Content-Type": "application/json" } })
			.then(response => response.json())
			.then(data => {
				this.setState({ name: data })
			});
	}

	componentDidMount() {
		this.refreshUsername();
	}

	render() {
		strings.setLanguage(this.state.language);
		const {
			name
		} = this.state;

        return (
			<header>

				<div className="header-top">
					<div className="header-name">
						<a href={Variables.MVC_URL}>TicketManagement</a>
					</div>

					{name === "" || name == null ?
						<a className="header-user-reference" href={Variables.MVC_URL + "Account/Login"}>{strings.login}</a>
						: <NavLink className="header-user-reference" to="/user">{ name }</NavLink>
					}
				</div>

				<div className="header-bottom">
					<div className="header-all-references-block">
						<a className="header-reference" href={Variables.MVC_URL}>{strings.main}</a>
						<a className="header-reference" href={Variables.MVC_URL + "Home/Poster"}>{strings.poster}</a>
						<a className="header-reference" href={Variables.MVC_URL + "Home/FAQ"} id="header_last_reference">{strings.faq}</a>
					</div>
				</div>

			</header>
        )
    }
}
