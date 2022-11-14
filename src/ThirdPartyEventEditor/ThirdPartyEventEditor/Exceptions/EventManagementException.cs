using System;

namespace ThirdPartyEventEditor.Exceptions
{
	/// <summary>
	/// Custom exception, thrown when wrong management third party event.
	/// </summary>
	public class EventManagementException : Exception
	{
		/// <summary>
		/// The name of the parameter that caused the exception to be thrown.
		/// </summary>
		public string GuiltyParam { get; private set; }

		/// <summary>
		/// Offer for fixed state.
		/// </summary>
		public string FixOffer { get; private set; }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="message">Message exception.</param>
		/// <param name="fixOffer">Offer for fixed state.</param>
		/// <param name="guiltyParamName">Guilty parameter name.</param>
		public EventManagementException(string message, string fixOffer, string guiltyParamName)
			: base(message)
		{
			GuiltyParam = guiltyParamName;
			FixOffer = fixOffer;
		}
	}
}