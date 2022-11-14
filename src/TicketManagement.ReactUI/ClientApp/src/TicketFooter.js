import React, { Component } from "react";
import ticket from './Ticket.ico'
import './header-footer.css'
import footerData from './localization/footerData';
import LocalizedStrings from 'react-localization';
import { read_cookie } from "sfcookies";

let strings = new LocalizedStrings(footerData);

export class TicketFooter extends Component {
	constructor(props) {
		super(props);

		this.state = {
			language: read_cookie("language"),
		}
	}

	render() {
		strings.setLanguage(this.state.language);
		return (
			<footer>
				<div className="footer-copy">
					<span className="footer-name">TicketManagement</span>
					<span>Copyright &copy; 2021 {strings.allRightsReserved}</span>
				</div>
				<div className="footer-img-block">
					<img src={ticket} />
				</div>
			</footer>
		)
	}
}





