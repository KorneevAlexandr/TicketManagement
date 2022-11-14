using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.FeatureManagement.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TicketManagement.WebApplication.FeatureHandlers
{
	public class RedirectDisabledFeatureHandler : IDisabledFeaturesHandler
	{
		private readonly string _userPath = "/user";
		private readonly string _basePath;

		public RedirectDisabledFeatureHandler(string basePath)
		{
			_basePath = basePath;
		}

		public Task HandleDisabledFeatures(IEnumerable<string> features, ActionExecutingContext context)
		{
			context.Result = new RedirectResult(_basePath + _userPath);
			return Task.CompletedTask;
		}
	}
}
