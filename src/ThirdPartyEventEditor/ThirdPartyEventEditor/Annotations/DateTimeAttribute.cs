using System;
using System.ComponentModel.DataAnnotations;

namespace ThirdPartyEventEditor.Annotations
{
	/// <summary>
	/// Annotation for check DateTime object.
	/// </summary>
	public class DateTimeAttribute : ValidationAttribute
	{
		public DateTimeAttribute()
		{ 
		}

		/// <summary>
		/// Checking if a string can be converted to an object DateTime.
		/// </summary>
		/// <param name="value">Value for checking.</param>
		/// <returns>True - valid, false - not valid.</returns>
		public override bool IsValid(object value)
		{
			bool result;
			if (value != null)
			{
				result = DateTime.TryParse(value.ToString(), out _);
			}
			else
			{
				result = false;
			}
			
			return result;
		}
	}
}