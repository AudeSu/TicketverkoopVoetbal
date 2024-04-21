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
    public class ZitplaatsService : IService<Zitplaat>
    {
        private IDAO<Zitplaat> _zitplaatsDAO;

        public ZitplaatsService(IDAO<Zitplaat> zitplaatsDAO)
        {
            _zitplaatsDAO = zitplaatsDAO;
        }

        public Task Add(Zitplaat entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(Zitplaat entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Zitplaat>?> FilterById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Zitplaat?> FindById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Zitplaat>?> GetAll()
        {
            return await _zitplaatsDAO.GetAll();
        }

        public Task Update(Zitplaat entity)
        {
            throw new NotImplementedException();
        }
    }
}
