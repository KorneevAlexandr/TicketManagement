using Microsoft.AspNetCore.Builder;
using TicketManagement.PurchaseAPI.Middleware;

namespace TicketManagement.PurchaseAPI.MiddlewareExtension
{
	/// <summary>
	/// Extensions for short use LogMiddleware.
	/// </summary>
	public static class LogRequestExtension
	{
		/// <summary>
		/// Method extension for connection middleware.
		/// </summary>
		/// <param name="builder">This application builder.</param>
		/// <returns>Application builder.</returns>
		public static IApplicationBuilder UseLogRequest(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<LogRequestMiddleware>();
		}
	}
}
