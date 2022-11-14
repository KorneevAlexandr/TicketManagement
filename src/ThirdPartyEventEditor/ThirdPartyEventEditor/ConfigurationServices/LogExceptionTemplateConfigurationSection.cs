using System.Configuration;

namespace ThirdPartyEventEditor.ConfigurationServices
{
	/// <summary>
	/// Represent access to 'logExceptionTemplate' section in web.config.
	/// </summary>
	public class LogExceptionTemplateConfigurationSection : ConfigurationSection
	{
		/// <summary>
		/// Represent management from 'titleException' attribute.
		/// </summary>
		[ConfigurationProperty("titleException")]
		public string ExceptionName
		{
			get { return (string)this["titleException"]; }
			set { this["titleException"] = value; }
		}

		/// <summary>
		/// Represent management from 'messageException' attribute.
		/// </summary>
		[ConfigurationProperty("messageException")]
		public string ExceptionMessage
		{
			get { return (string)this["messageException"]; }
			set { this["messageException"] = value; }
		}

		/// <summary>
		/// Represent management from 'param' attribute.
		/// </summary>
		[ConfigurationProperty("param")]
		public string Param
		{
			get { return (string)this["param"]; }
			set { this["param"] = value; }
		}

		/// <summary>
		/// Represent management from 'offer' attribute.
		/// </summary>
		[ConfigurationProperty("offer")]
		public string Offer
		{
			get { return (string)this["offer"]; }
			set { this["offer"] = value; }
		}

		/// <summary>
		/// Generate string template, use configuration template for exception.
		/// Using for system exception.
		/// </summary>
		/// <param name="exceptionName">Exception name or type.</param>
		/// <param name="message">Exception message.</param>
		/// <returns>String template.</returns>
		public string FillingTemplate(string exceptionName, string message)
		{
			var data = string.Concat("\n", this.ExceptionName, exceptionName, "\n",
						 this.ExceptionMessage, message, "\n\n");

			return data;
		}

		/// <summary>
		/// Generate string template, use configuration template for exception.
		/// Using for custom-user exception (recommended for EventMamangementException).
		/// </summary>
		/// <param name="exceptionName">Exception name or type.</param>
		/// <param name="message">Exception message.</param>
		/// <param name="offer">Offer for fixed problem.</param>
		/// <param name="param">Guilty parameter.</param>
		/// <returns>String template.</returns>
		public string FillingTemplate(string exceptionName, string message, string offer, string param)
		{
			var data = string.Concat("\n", this.ExceptionName, exceptionName, "\n",
						 this.ExceptionMessage, message, "\n",
						 this.Offer, offer, "\n",
						 Param, param, "\n\n");

			return data;
		}
	}
}