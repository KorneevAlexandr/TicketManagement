using System;
using TicketManagement.DataAccess.Domain;

namespace TicketManagement.BusinessLogic.ValidationProviders
{
	/// <summary>
	/// Represents static methods for check validation state Venues.
	/// </summary>
	internal static class VenueValidationProvider
	{
		/// <summary>
		/// Check validation state Venue when use Add-method.
		/// </summary>
		/// <param name="venue">Venue.</param>
		public static void IsValidToAdd(Venue venue)
		{
			if (venue == null)
			{
				throw new ArgumentNullException("venue", "Can not add null venue.");
			}
			if (string.IsNullOrWhiteSpace(venue.Description) || (venue.Description.Length > 120))
			{
				throw new ArgumentException(
					"Invalid Description: Description does not empty and longer then 120 characters.");
			}
			if (string.IsNullOrWhiteSpace(venue.Address) || (venue.Address.Length > 200))
			{
				throw new ArgumentException(
					"Invalid Address: Address does not empty and longer then 200 characters.");
			}

			// phone may be is null
			if (venue.Phone.Length > 30)
			{
				throw new ArgumentException(
					"Invalid Phone: Phone does not longer then 30 characters.");
			}
			if (string.IsNullOrWhiteSpace(venue.Name) || (venue.Name.Length > 120))
			{
				throw new ArgumentException(
					"Invalid Name: Name does not empty and longer then 120 characters.");
			}
		}

		/// <summary>
		/// Check validation state EventArea when use Delete-method.
		/// </summary>
		/// <param name="id">Id.</param>
		public static void IsValidToDelete(int id)
		{
			if (id <= 0)
			{
				throw new ArgumentException("Venue id cannot be 0 or less.");
			}
		}

		/// <summary>
		/// Check validation state Venue when use Update-method.
		/// </summary>
		/// <param name="venue">Venue.</param>
		public static void IsValidToUpdate(Venue venue)
		{
			if (venue == null)
			{
				throw new ArgumentNullException("venue", "Can not update null venue.");
			}
			if (venue.Id <= 0)
			{
				throw new ArgumentException("Invalid Id: Id cannot be less 1.");
			}
			if (string.IsNullOrWhiteSpace(venue.Description) || (venue.Description.Length > 120))
			{
				throw new ArgumentException(
					"Invalid Description: Description does not empty and longer then 120 characters.");
			}
			if (string.IsNullOrWhiteSpace(venue.Address) || (venue.Address.Length > 200))
			{
				throw new ArgumentException(
					"Invalid Address: Address does not empty and longer then 200 characters.");
			}

			// phone may be is null
			if (venue.Phone.Length > 30)
			{
				throw new ArgumentException(
					"Invalid Phone: Phone does not longer then 30 characters.");
			}
			if (string.IsNullOrWhiteSpace(venue.Name) || (venue.Name.Length > 120))
			{
				throw new ArgumentException(
					"Invalid Name: Name does not empty and longer then 120 characters.");
			}
		}
	}
}
