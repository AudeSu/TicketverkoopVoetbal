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
    public class StadionService : IService<Stadion>
    {
        private IDAO<Stadion> _stadionDAO;

        public StadionService(IDAO<Stadion> stadionDAO)
        {
            _stadionDAO = stadionDAO;
        }

        public Task Add(Stadion entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(Stadion entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Stadion>?> FindById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Stadion>?> GetAll()
        {
            return await _stadionDAO.GetAll();
        }

        public Task Update(Stadion entity)
        {
            throw new NotImplementedException();
        }
    }
}
