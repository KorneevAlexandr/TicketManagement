using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.WebApplication.Middleware;

namespace TicketManagement.WebApplication.MiddlewareExtension
{
	/// <summary>
	/// Extension for short use LanguageTransferMiddleware.
	/// </summary>
	public static class LanguageTransferExtension
	{
		/// <summary>
		/// Method extension for connection middleware.
		/// </summary>
		/// <param name="builder">This application builder.</param>
		/// <returns>Application builder.</returns>
		public static IApplicationBuilder UseLanguageTransfer(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<LanguageTransferMiddleware>();
		}
	}
}
