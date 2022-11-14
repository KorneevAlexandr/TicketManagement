using System;
using TicketManagement.DataAccess.Domain;

namespace TicketManagement.EventAPI.Services.ValidationProviders
{
	/// <summary>
	/// Represents static methods for check validation state EventSeats.
	/// </summary>
	internal static class EventSeatValidationProvider
	{
		/// <summary>
		/// Check validation state EventArea when use Delete-method.
		/// </summary>
		/// <param name="id">Id.</param>
		public static void IsValidToDelete(int id)
		{
			if (id <= 0)
			{
				throw new ArgumentException("Seat id cannot be 0 or less.");
			}
		}

		/// <summary>
		/// Check validation state EventSeat when use Update-method.
		/// </summary>
		/// <param name="seat">EventSeat.</param>
		public static void IsValidToUpdate(EventSeat seat)
		{
			if (seat == null)
			{
				throw new ArgumentNullException("seat", "Can not update null seat.");
			}

			if (seat.Id <= 0)
			{
				throw new ArgumentException("Invalid Id: Id cannot be less 1.");
			}
			if (seat.EventAreaId <= 0)
			{
				throw new ArgumentException("Invalid EventAreaId: such eventArea does not exist.");
			}
			if (seat.Row <= 0)
			{
				throw new ArgumentException("Invalid Row: Row cannot be less 1.");
			}
			if (seat.Number <= 0)
			{
				throw new ArgumentException("Invalid Number: Number cannot be less 1.");
			}
		}
	}
}
