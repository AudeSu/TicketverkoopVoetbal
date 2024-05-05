using TicketverkoopVoetbal.Domains.Entities;
using TicketverkoopVoetbal.Repositories;
using TicketverkoopVoetbal.Repositories.Interfaces;
using TicketverkoopVoetbal.Services.Interfaces;

namespace TicketverkoopVoetbal.Services
{
    public class StoeltjeService : IService<Stoeltje>
    {
        private IDAO<Stoeltje> _stoeltjeDAO;

        public StoeltjeService(IDAO<Stoeltje> stoeltjeDAO)
        {
            _stoeltjeDAO = stoeltjeDAO;
        }

        public async Task Add(Stoeltje entity)
        {
            await _stoeltjeDAO.Add(entity);
        }

        public Task Delete(Stoeltje entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Stoeltje>?> FilterById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Stoeltje?> FindById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Stoeltje>?> GetAll()
        {
            return await _stoeltjeDAO.GetAll();
        }

        public Task Update(Stoeltje entity)
        {
            throw new NotImplementedException();
        }
    }
}
