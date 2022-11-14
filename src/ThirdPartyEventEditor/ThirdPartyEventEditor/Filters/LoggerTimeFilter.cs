using System.IO;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using ThirdPartyEventEditor.ConfigurationServices;

namespace ThirdPartyEventEditor.Filters
{
	/// <summary>
	/// Represents possibility management log.
	/// </summary>
	public class LoggerTimeFilter : ActionFilterAttribute, IActionFilter
	{
		private static readonly LogTemplateConfigurationSection _logTemplate;
		private static bool _isStarted = false;

		static LoggerTimeFilter()
		{
			_logTemplate = ((CustomLogConfigurationGroup)WebConfigurationManager.OpenWebConfiguration("/")
				.GetSectionGroup("customLog")).LogTemplate;
		}

		private static bool IsStarted
		{
			get
			{
				var started = _isStarted;
				_isStarted = _isStarted ? _isStarted : true;
				return started;
			}
		}

		/// <summary>
		/// Register and write action time in log-file.
		/// </summary>
		/// <param name="filterContext">Context for write.</param>
		void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
		{
			var controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
			var actionName = filterContext.ActionDescriptor.ActionName;
			var time = filterContext.HttpContext.Timestamp;
			var endTime = IsStarted ? time.ToString() : null;
			var data = _logTemplate.FillingTemplate(controllerName, actionName, time.ToString(), endTime);

			string fileName = WebConfigurationManager.AppSettings["LogSourcePath"];
			string path = HttpContext.Current.Server.MapPath(fileName);
			
			var stream = new StreamWriter(path, true);
			stream.WriteAsync(data).Wait();
			stream.Close();

			OnActionExecuting(filterContext);
		}
	}
}