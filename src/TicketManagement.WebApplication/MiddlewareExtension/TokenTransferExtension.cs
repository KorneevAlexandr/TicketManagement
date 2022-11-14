using Microsoft.AspNetCore.Builder;
using TicketManagement.WebApplication.Middleware;

namespace TicketManagement.WebApplication.MiddlewareExtension
{
	/// <summary>
	/// Extensions for short use TokenTransferMiddleware.
	/// </summary>
	public static class TokenTransferExtension
	{
		/// <summary>
		/// Method extension for connection middleware.
		/// </summary>
		/// <param name="builder">This application builder.</param>
		/// <returns>Application builder.</returns>
		public static IApplicationBuilder UseTokenTransferring(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<TokenTransferMiddleware>();
		}
	}
}
