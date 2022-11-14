using System;
using TicketManagement.DataAccess.Domain;

namespace TicketManagement.BusinessLogic.ValidationProviders
{
	/// <summary>
	/// Represents static methods for check validation state Areas.
	/// </summary>
	internal static class AreaValidationProvider
	{
		/// <summary>
		/// Check validation state Area when use Add-method.
		/// </summary>
		/// <param name="area">Area.</param>
		public static void IsValidToAdd(Area area)
		{
			if (area == null)
			{
				throw new ArgumentNullException("area", "Can not add null area.");
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

			if (area.LayoutId <= 0)
			{
				throw new ArgumentException("Invalid LayoutId: such layout does not exist.");
			}
		}

		/// <summary>
		/// Check validation state Area when use Delete-method.
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
		/// Check validation state Area when use Update-method.
		/// </summary>
		/// <param name="area">Area.</param>
		public static void IsValidToUpdate(Area area)
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

			if (area.LayoutId <= 0)
			{
				throw new ArgumentException("Invalid LayoutId: such layout does not exist.");
			}
		}
	}
}
