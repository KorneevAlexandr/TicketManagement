using System;
using TicketManagement.DataAccess.Domain;

namespace TicketManagement.BusinessLogic.ValidationProviders
{
	/// <summary>
	/// Represents static methods for check validation state Events.
	/// </summary>
	internal static class EventValidationProvider
	{
		/// <summary>
		/// Check validation state Event when use Add-method.
		/// </summary>
		/// <param name="userEvent">Event.</param>
		public static void IsValidToAdd(Event userEvent)
		{
			if (userEvent == null)
			{
				throw new ArgumentNullException("userEvent", "Can not add null event.");
			}
			if (string.IsNullOrWhiteSpace(userEvent.Name) || (userEvent.Name.Length > 120))
			{
				throw new ArgumentException(
					"Invalid Name: Name does not empty and longer then 120 characters.");
			}
			if (string.IsNullOrWhiteSpace(userEvent.Description))
			{
				throw new ArgumentException("Invalid Description: Description does not empty.");
			}

			if (userEvent.Price < 0)
			{
				throw new ArgumentException("Invalid Price: Price does not negative.");
			}

			if (userEvent.DateTimeStart < DateTime.UtcNow || userEvent.DateTimeEnd < DateTime.UtcNow)
			{
				throw new ArgumentException(
					"Invalid DateTime: date and time event does not created in past.");
			}
			if (userEvent.DateTimeStart >= userEvent.DateTimeEnd)
			{
				throw new ArgumentException(
					"Invalid DateTime: the end of an event cannot be earlier than its start.");
			}

			if (userEvent.LayoutId <= 0)
			{
				throw new ArgumentException("Invalid LayoutId: such layout does not exist.");
			}
		}

		/// <summary>
		/// Check validation state Event when use Delete-method.
		/// </summary>
		/// <param name="id">Id.</param>
		public static void IsValidToDelete(int id)
		{
			if (id <= 0)
			{
				throw new ArgumentException("Event id cannot be 0 or less.");
			}
		}

		/// <summary>
		/// Check validation state Event when use Update-method.
		/// </summary>
		/// <param name="userEvent">Event.</param>
		public static void IsValidToUpdate(Event userEvent)
		{
			if (userEvent == null)
			{
				throw new ArgumentNullException("userEvent", "Can not update null event.");
			}
			if (userEvent.Id <= 0)
			{
				throw new ArgumentException("Invalid Id: Id cannot be less 1.");
			}
			if (string.IsNullOrWhiteSpace(userEvent.Name) || (userEvent.Name.Length > 120))
			{
				throw new ArgumentException(
					"Invalid Name: Name does not empty and longer then 120 characters.");
			}
			if (string.IsNullOrWhiteSpace(userEvent.Description))
			{
				throw new ArgumentException("Invalid Description: Description does not empty.");
			}

			if (userEvent.DateTimeStart < DateTime.UtcNow || userEvent.DateTimeEnd < DateTime.UtcNow)
			{
				throw new ArgumentException(
					"Invalid DateTime: date and time event does not created in past.");
			}
			if (userEvent.DateTimeStart >= userEvent.DateTimeEnd)
			{
				throw new ArgumentException(
					"Invalid DateTime: the end of an event cannot be earlier than its start.");
			}

			if (userEvent.Price < 0)
			{
				throw new ArgumentException("Invalid Price: Price does not negative.");
			}

			if (userEvent.LayoutId <= 0)
			{
				throw new ArgumentException("Invalid LayoutId: such layout does not exist.");
			}
		}

		public static void IsValidEventToPlaces(int id)
		{
			if (id <= 0)
			{
				throw new ArgumentException("Invalid Id: Id cannot be less 1.");
			}
		}
	}
}
