using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace TicketManagement.EventAPI.Middleware
{
	/// <summary>
	/// Middleware, logging all information about incoming requests.
	/// </summary>
	public class LogRequestMiddleware
	{
		private readonly RequestDelegate _next;

		/// <summary>
		/// Initializes a new instance of the <see cref="LogRequestMiddleware"/> class.
		/// </summary>
		/// <param name="next">Next middleware.</param>
		public LogRequestMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		/// <summary>
		/// Logs all information about incoming requests and calls the next middleware.
		/// </summary>
		/// <param name="context">HttpContext.</param>
		/// <returns>Task.</returns>
		public async Task InvokeAsync(HttpContext context)
		{
			var request = context.Request;

			var userName = context.User.Identity.Name == string.Empty ? "anonymous" : context.User.Identity.Name;
			var claimRole = ((ClaimsIdentity)context.User.Identity).Claims
				.FirstOrDefault(x => x.Type == ClaimTypes.Role);
			var userRole = claimRole == null ? "anonymous" : claimRole.Value;

			var logString = string.Concat("Host: ", request.Host, "; ",
										  "Path: ", request.Path, "; ",
										  "Method: ", request.Method, "; ",
										  "User name: ", context.User.Identity.Name, "; ",
										  "User role: ", userRole, "; ",
										  "Content type: ", request.ContentType, ", ",
										  "Schema: ", request.Scheme, "; ",
										  "Protocol: ", request.Protocol, "; ",
										  "Response status code: ", context.Response.StatusCode, ".");

			Log.Information(logString);
			await _next.Invoke(context);
		}

	}
}
