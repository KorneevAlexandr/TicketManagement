using System.IO;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using ThirdPartyEventEditor.ConfigurationServices;
using ThirdPartyEventEditor.Exceptions;

namespace ThirdPartyEventEditor.Filters
{
	/// <summary>
	/// Filter for aplication exception.
	/// </summary>
	public class ApplicationExceptionFilter : FilterAttribute, IExceptionFilter
	{
		private static readonly LogExceptionTemplateConfigurationSection _logExceptionTemplate;

		static ApplicationExceptionFilter()
		{
			_logExceptionTemplate = ((CustomLogConfigurationGroup)WebConfigurationManager.OpenWebConfiguration("/")
				.GetSectionGroup("customLog")).LogExceptionTemplate;
		}

		/// <summary>
		/// Register EventManagementException and general exceptions. Redirect to general page and write 
		/// info about exception in log-file.
		/// </summary>
		/// <param name="filterContext">Filter context.</param>
		public void OnException(ExceptionContext filterContext)
		{
			if (!filterContext.ExceptionHandled && filterContext.Exception is EventManagementException exception)
			{
				var fixOffer = exception.FixOffer;
				var param = exception.GuiltyParam;
				var data = _logExceptionTemplate.FillingTemplate(exception.GetType().ToString(), exception.Message, fixOffer, param);
				
				WriteExceptionToLogFile(data);
				filterContext.Result = new RedirectResult(string.Concat("~/Home/Error?title=", filterContext.Exception.GetType().ToString(),
														"&message=", string.Concat(filterContext.Exception.Message, " ", fixOffer)));
				filterContext.ExceptionHandled = true;
			}

			if (!filterContext.ExceptionHandled)
			{
				var data = _logExceptionTemplate.FillingTemplate(filterContext.Exception.GetType().ToString(), filterContext.Exception.Message);
				WriteExceptionToLogFile(data);

				filterContext.Result = new RedirectResult(string.Concat("~/Home/Error?title=", filterContext.Exception.GetType().ToString(),
																		"&message=", filterContext.Exception.Message));
				filterContext.ExceptionHandled = true;
			}
		}

		private void WriteExceptionToLogFile(string data)
		{
			string fileName = WebConfigurationManager.AppSettings["LogSourcePath"];
			string path = HttpContext.Current.Server.MapPath(fileName);

			var stream = new StreamWriter(path, true);
			stream.WriteAsync(data).Wait();
			stream.Close();
		}
	}
}