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
    public class ClubService : IService<Club>
    {
        private IDAO<Club> _clubDAO;

        public ClubService(IDAO<Club> clubDAO)
        {
            _clubDAO = clubDAO;
        }

        public Task Add(Club entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(Club entity)
        {
            throw new NotImplementedException();
        }

        public async Task<Club?> FindById(int id)
        {
            return await _clubDAO.FindById(id);
        }

        public async Task<IEnumerable<Club>?> GetAll()
        {
            return await _clubDAO.GetAll();
        }

        public Task Update(Club entity)
        {
            throw new NotImplementedException();
        }


        Task<IEnumerable<Club>?> IService<Club>.FilterById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
