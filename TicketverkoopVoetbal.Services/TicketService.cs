using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketverkoopVoetbal.Domains.Entities;
using TicketverkoopVoetbal.Repositories;
using TicketverkoopVoetbal.Repositories.Interfaces;
using TicketverkoopVoetbal.Services.Interfaces;

namespace TicketverkoopVoetbal.Services
{
    public class TicketService : IService<Ticket>
    {
        private IDAO<Ticket> _ticketDAO;

        public TicketService(IDAO<Ticket> ticketDAO)
        {
            _ticketDAO = ticketDAO;
        }

        public async Task Add(Ticket entity)
        {
            await _ticketDAO.Add(entity);
        }

        public Task Delete(Ticket entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Ticket>?> FilterById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Ticket?> FindById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Ticket>?> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task Update(Ticket entity)
        {
            throw new NotImplementedException();
        }
    }
}
