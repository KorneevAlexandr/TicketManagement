using System.Configuration;

namespace ThirdPartyEventEditor.ConfigurationServices
{
	/// <summary>
	/// Represent access to 'logTemplate' section in web.config.
	/// </summary>
	public class LogTemplateConfigurationSection : ConfigurationSection
	{
		/// <summary>
		/// Represent management from 'controllerName' attribute.
		/// </summary>
		[ConfigurationProperty("controllerName")]
		public string Controller
		{
			get { return (string)this["controllerName"]; }
			set { this["contollerName"] = value; }
		}

		/// <summary>
		/// Represent management from 'actionName' attribute.
		/// </summary>
		[ConfigurationProperty("actionName")]
		public string Action
		{
			get { return (string)this["actionName"]; }
			set { this["actionName"] = value; }
		}

		/// <summary>
		///  Represent management from 'timeStart' attribute.
		/// </summary>
		[ConfigurationProperty("timeStart")]
		public string TimeStart
		{
			get { return (string)this["timeStart"]; }
			set { this["timeStart"] = value; }
		}

		/// <summary>
		///  Represent management from 'timeEnd' attribute.
		/// </summary>
		[ConfigurationProperty("timeEnd")]
		public string TimeEnd
		{
			get { return (string)this["timeEnd"]; }
			set { this["timeEnd"] = value; }
		}

		/// <summary>
		/// Represent management from 'exit' attribute.
		/// Used when exit time for action unknown (where application stop).
		/// </summary>
		[ConfigurationProperty("exit")]
		public string Exit
		{
			get { return (string)this["exit"]; }
			set { this["exit"] = value; }
		}

		/// <summary>
		/// Generate string template, use configuration template for simple logging runtime actions.
		/// </summary>
		/// <param name="controllerName">Controller name.</param>
		/// <param name="actionName">Action name.</param>
		/// <param name="timeStart">Time callback new action.</param>
		/// <param name="timeEnd">Time exit from old action.</param>
		/// <returns>String template.</returns>
		public string FillingTemplate(string controllerName, string actionName, string timeStart, string timeEnd)
		{
			timeEnd = timeEnd == null || timeEnd == string.Empty ? Exit : timeEnd;
			var data = string.Concat(this.TimeEnd, timeEnd, "\n\n",
						 this.Controller, controllerName, "\n",
						 this.Action, actionName, "\n",
						 this.TimeStart, timeStart.ToString(), "\n");
			return data;
		}
	}
}