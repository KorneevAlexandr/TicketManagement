using System;
using TicketManagement.DataAccess.Domain;

namespace TicketManagement.BusinessLogic.ValidationProviders
{
	/// <summary>
	/// Represents static methods for check validation state Layouts.
	/// </summary>
	internal static class LayoutValidationProvider
	{
		/// <summary>
		/// Check validation state Layout when use Add-method.
		/// </summary>
		/// <param name="layout">Layout.</param>
		public static void IsValidToAdd(Layout layout)
		{
			if (layout == null)
			{
				throw new ArgumentNullException("layout", "Can not add null layout.");
			}

			if (string.IsNullOrWhiteSpace(layout.Description) || (layout.Description.Length > 120))
			{
				throw new ArgumentException(
					"Invalid Description: Description does not empty and longer then 120 characters.");
			}

			if (string.IsNullOrWhiteSpace(layout.Name) || (layout.Name.Length > 120))
			{
				throw new ArgumentException(
					"Invalid Name: Name does not empty and longer then 120 characters.");
			}

			if (layout.VenueId <= 0)
			{
				throw new ArgumentException("Invalid VenueId: such venue does not exist.");
			}
		}

		/// <summary>
		/// Check validation state Layout when use Delete-method.
		/// </summary>
		/// <param name="id">Id.</param>
		public static void IsValidToDelete(int id)
		{
			if (id <= 0)
			{
				throw new ArgumentException("Layout id cannot be 0 or less.");
			}
		}

		/// <summary>
		/// Check validation state Layout when use Update-method.
		/// </summary>
		/// <param name="layout">Layout.</param>
		public static void IsValidToUpdate(Layout layout)
		{
			if (layout == null)
			{
				throw new ArgumentNullException("layout", "Can not update null layout.");
			}
			if (layout.Id <= 0)
			{
				throw new ArgumentException("Invalid Id: Id cannot be less 1.");
			}

			if (string.IsNullOrWhiteSpace(layout.Description) || (layout.Description.Length > 120))
			{
				throw new ArgumentException(
					"Invalid Description: Description does not empty and longer then 120 characters.");
			}

			if (string.IsNullOrWhiteSpace(layout.Name) || (layout.Name.Length > 120))
			{
				throw new ArgumentException(
					"Invalid Name: Name does not empty and longer then 120 characters.");
			}

			if (layout.VenueId <= 0)
			{
				throw new ArgumentException("Invalid VenueId: such venue does not exist.");
			}
		}
 	}
}
