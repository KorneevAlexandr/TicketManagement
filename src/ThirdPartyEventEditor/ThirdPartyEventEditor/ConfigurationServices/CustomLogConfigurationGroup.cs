using System.Configuration;

namespace ThirdPartyEventEditor.ConfigurationServices
{
	/// <summary>
	/// Represent access to 'customLog group' in web.config.
	/// </summary>
	public class CustomLogConfigurationGroup : ConfigurationSectionGroup
	{
		/// <summary>
		/// Geeting 'logTemplate' section from web.config.
		/// </summary>
		public LogTemplateConfigurationSection LogTemplate
		{
			get
			{
				return (LogTemplateConfigurationSection)Sections["logTemplate"];
			}
		}

		/// <summary>
		/// Geeting 'logExceptionTemplate' section from web.config.
		/// </summary>
		public LogExceptionTemplateConfigurationSection LogExceptionTemplate
		{
			get
			{
				return (LogExceptionTemplateConfigurationSection)Sections["logExceptionTemplate"];
			}
		}
	}
}