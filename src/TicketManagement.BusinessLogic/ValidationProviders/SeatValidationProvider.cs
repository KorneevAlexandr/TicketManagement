using System;
using TicketManagement.DataAccess.Domain;

namespace TicketManagement.BusinessLogic.ValidationProviders
{
	/// <summary>
	/// Represents static methods for check validation state Seats.
	/// </summary>
	internal static class SeatValidationProvider
	{
		/// <summary>
		/// Check validation state Seat when use Add-method.
		/// </summary>
		/// <param name="seat">Seat.</param>
		public static void IsValidToAdd(Seat seat)
		{
			if (seat == null)
			{
				throw new ArgumentNullException("seat", "Can not add null seat.");
			}

			if (seat.AreaId <= 0)
			{
				throw new ArgumentException("Invalid AreaId: such area does not exist.");
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

		/// <summary>
		/// Check validation state Seat when use Delete-method.
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
		/// Check validation state Seat when use Update-method.
		/// </summary>
		/// <param name="seat">Seat.</param>
		public static void IsValidToUpdate(Seat seat)
		{
			if (seat == null)
			{
				throw new ArgumentNullException("seat", "Can not update null seat.");
			}

			if (seat.Id <= 0)
			{
				throw new ArgumentException("Invalid Id: Id cannot be less 1.");
			}
			if (seat.AreaId <= 0)
			{
				throw new ArgumentException("Invalid AreaId: such area does not exist.");
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
