using System;
using TicketManagement.DataAccess.Domain;

namespace TicketManagement.EventAPI.Services.ValidationProviders
{
	/// <summary>
	/// Represents static methods for check validation state EventAreas.
	/// </summary>
	internal static class EventAreaValidationProvider
	{
		/// <summary>
		/// Check validation state EventArea when use Delete-method.
		/// </summary>
		/// <param name="id">Id.</param>
		public static void IsValidToDelete(int id)
		{
			if (id <= 0)
			{
				throw new ArgumentException("Area id cannot be 0 or less.");
			}
		}

		/// <summary>
		/// Check validation state EventArea when use Update-method.
		/// </summary>
		/// <param name="area">EventArea.</param>
		public static void IsValidToUpdate(EventArea area)
		{
			if (area == null)
			{
				throw new ArgumentNullException("area", "Can not update null area.");
			}

			if (area.Id <= 0)
			{
				throw new ArgumentException("Invalid Id: Id cannot be less 1.");
			}

			if (string.IsNullOrWhiteSpace(area.Description) || (area.Description.Length > 200))
			{
				throw new ArgumentException(
					"Invalid Description: Description does not empty and longer then 200 characters.");
			}
			if (area.CoordX <= 0)
			{
				throw new ArgumentException("Invalid CoordX: CoordX cannot be less 1.");
			}
			if (area.CoordY <= 0)
			{
				throw new ArgumentException("Invalid CoordY: CoordY cannot be less 1.");
			}
			if (area.Price < 0)
			{
				throw new ArgumentException("Invalid Price: Price cannot be less 0.");
			}

			if (area.EventId <= 0)
			{
				throw new ArgumentException("Invalid EventId: such event does not exist.");
			}
		}
	}
}
