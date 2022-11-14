using System.Web;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Web.Configuration;

namespace ThirdPartyEventEditor.Annotations
{
	/// <summary>
	/// Annotation for checking existence image.
	/// </summary>
	public class InnerImageAttribute : ValidationAttribute
	{
		public InnerImageAttribute()
		{ 
		}

		/// <summary>
		/// Checking existence image in specified image.
		/// Searching in directory, specified in Web.confing.
		/// </summary>
		/// <param name="value">Object for checking.</param>
		/// <returns>True - exist, false - not exist.</returns>
		public override bool IsValid(object value)
		{
			var result = false;
			if (value != null)
			{
				try
				{
					var fileName = WebConfigurationManager.AppSettings["ImagesSourcePath"];
					var path = Path.Combine(HttpContext.Current.Server.MapPath(fileName), value.ToString());
					if (File.Exists(path))
					{
						result = true;
					}
				}
				catch
				{
					result = false;
				}
			}

			return result;
		}

	}
}