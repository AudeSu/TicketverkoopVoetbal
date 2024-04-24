using TicketverkoopVoetbal.Domains.Entities;
using TicketverkoopVoetbal.Repositories.Interfaces;
using TicketverkoopVoetbal.Services.Interfaces;

namespace TicketverkoopVoetbal.Services
{
    public class ZitplaatsService : IService<Zitplaats>
    {
        private IDAO<Zitplaats> _zitplaatsDAO;

        public ZitplaatsService(IDAO<Zitplaats> zitplaatsDAO)
        {
            _zitplaatsDAO = zitplaatsDAO;
        }

        public Task Add(Zitplaats entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(Zitplaats entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Zitplaats>?> FilterById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Zitplaats?> FindById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Zitplaats>?> GetAll()
        {
            return await _zitplaatsDAO.GetAll();
        }

        public Task Update(Zitplaats entity)
        {
            throw new NotImplementedException();
        }
    }
}
