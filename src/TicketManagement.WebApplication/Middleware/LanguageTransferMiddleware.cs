using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace TicketManagement.WebApplication.Middleware
{
	/// <summary>
	/// Checks cookies containing language indication.
	/// </summary>
	public class LanguageTransferMiddleware
	{
		private const string LanguageCookieKey = "language";
		private readonly RequestDelegate _next;

		/// <summary>
		/// Initializes a new instance of the <see cref="LanguageTransferMiddleware"/> class.
		/// </summary>
		/// <param name="next">Next middleware.</param>
		public LanguageTransferMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		/// <summary>
		/// Read cookies for getting language and calls the next middleware.
		/// </summary>
		/// <param name="context">HttpContext.</param>
		/// <returns>Task.</returns>
		public async Task InvokeAsync(HttpContext context)
		{
			string language = null;
			if (context.Request.Cookies.TryGetValue(LanguageCookieKey, out string value))
			{
				language = value;
			}

			if (language != null)
			{
				language = language.Contains("en") ? "en-US" : language;
				language = language.Contains("ru") ? "ru-RU" : language;
				language = language.Contains("be") ? "be-BY" : language;

				context.Response.Cookies.Append(
					CookieRequestCultureProvider.DefaultCookieName,
					CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(language)),
					new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });
			}

			await _next.Invoke(context);
		}
	}
}
