using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.BusinessLogic.BLInterfaces;
using TicketManagement.BusinessLogic.ModelsDTO;
using TicketManagement.DataAccess.Domain;
using TicketManagement.DataAccess.Interfaces.Repositories;

namespace TicketManagement.BusinessLogic.Services
{
	/// <summary>
	/// Represents operations for proxy ticket-repository calls with validation logic.
	/// </summary>
	internal class TicketService : ITicketService
	{
		private readonly ITicketRepository _ticketRepository;

		public TicketService(ITicketRepository ticketRepository)
		{
			_ticketRepository = ticketRepository;
		}

		/// <summary>
		/// Getting all tickets by user entity id.
		/// </summary>
		/// <param name="parentId">Parent (user) id.</param>
		/// <returns>Collections tickets.</returns>
		public async Task<IEnumerable<TicketDto>> GetAllAsync(int parentId)
		{
			var tickets = await _ticketRepository.GetAllAsync(parentId);

			return tickets
				.Select(ticket => new TicketDto
				{
					Id = ticket.Id,
					DateTimePurchase = ticket.DateTimePurchase,
					EventSeatId = ticket.EventSeatId,
					UserId = ticket.UserId,
					EventName = ticket.EventName,
					Price = ticket.Price,
				}).AsEnumerable();
		}

		/// <summary>
		/// Gettin ticket to data set.
		/// </summary>
		/// <param name="id">Id ticket to getting.</param>
		/// <returns>Ticket.</returns>
		public async Task<TicketDto> GetAsync(int id)
		{
			var ticket = await _ticketRepository.GetAsync(id);
			return new TicketDto
			{
				Id = ticket.Id,
				DateTimePurchase = ticket.DateTimePurchase,
				EventSeatId = ticket.EventSeatId,
				UserId = ticket.UserId,
				EventName = ticket.EventName,
				Price = ticket.Price,
			};
		}

		/// <summary>
		/// Add ticket in data set.
		/// </summary>
		/// <param name="entity">Ticket to add.</param>
		/// <returns>True - succesfull, false - fail.</returns>
		public async Task<bool> AddAsync(TicketDto entity)
		{
			return await _ticketRepository.AddAsync(ConvertToTicket(entity));
		}

		/// <summary>
		/// Delete ticket from data set.
		/// </summary>
		/// <param name="id">Id ticket to delete.</param>
		/// <returns>True - succesfull, false - fail.</returns>
		public bool Delete(int id)
		{
			return _ticketRepository.Delete(id);
		}

		/// <summary>
		/// Update ticket entity in data set.
		/// </summary>
		/// <param name="entity">Ticket to update.</param>
		/// <returns>True - succesfull, false - fail.</returns>
		public bool Update(TicketDto entity)
		{
			return _ticketRepository.Update(ConvertToTicket(entity));
		}

		private Ticket ConvertToTicket(TicketDto ticket)
		{
			return new Ticket
			{
				Id = ticket.Id,
				DateTimePurchase = ticket.DateTimePurchase,
				EventSeatId = ticket.EventSeatId,
				UserId = ticket.UserId,
				EventName = ticket.EventName,
				Price = ticket.Price,
			};
		}
	}
}
