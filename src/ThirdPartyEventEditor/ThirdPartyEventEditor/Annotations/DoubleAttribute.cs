using System.ComponentModel.DataAnnotations;

namespace ThirdPartyEventEditor.Annotations
{
	/// <summary>
	/// Annotation for check double object.
	/// </summary>
	public class DoubleAttribute : ValidationAttribute
	{
		private readonly double _minValue;

		public DoubleAttribute(double minValue)
		{
			_minValue = minValue;
		}

		public DoubleAttribute() : this(double.MinValue)
		{ 
		}

		/// <summary>
		/// Checking if a string can be converted to an object double.
		/// </summary>
		/// <param name="value">Value for checking.</param>
		/// <returns>True - valid, false - not valid.</returns>
		public override bool IsValid(object value)
		{
			var result = false;
			if (value != null)
			{
				if (double.TryParse(value.ToString(), out double valueResult) && valueResult >= _minValue)
				{
					result = true;
				}
			}
			return result;
		}
	}
}