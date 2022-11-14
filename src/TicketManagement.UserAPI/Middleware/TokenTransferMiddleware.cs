using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TicketManagement.UserAPI.Middleware
{
	/// <summary>
	/// Middleware, adds the user's jwt token from the request cookie to the request header.
	/// </summary>
	public class TokenTransferMiddleware
	{
		private readonly RequestDelegate _next;

		/// <summary>
		/// Initializes a new instance of the <see cref="TokenTransferMiddleware"/> class.
		/// </summary>
		/// <param name="next">Next middleware.</param>
		public TokenTransferMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		/// <summary>
		/// Overwrites the token from the request header.
		/// </summary>
		/// <param name="context">HttpContext.</param>
		/// <returns>Task.</returns>
		public async Task InvokeAsync(HttpContext context)
		{
			var token = context.Request.Headers["Token"];
			if (!string.IsNullOrEmpty(token))
				context.Request.Headers.Add("Authorization", "Bearer " + token);

			await _next.Invoke(context);
		}
	}
}
