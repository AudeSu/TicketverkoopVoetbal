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
    public class AbonnementService : IAbonnementService<Abonnement>
    {
        private IAbonnementDAO<Abonnement> _abonnementDAO;

        public AbonnementService(IAbonnementDAO<Abonnement> abonnementDAO)
        {
           _abonnementDAO = abonnementDAO;
        }

        public async Task Add(Abonnement entity)
        {
            await _abonnementDAO.Add(entity);
        }

        public Task Delete(Abonnement entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Abonnement>?> FilterById(int id)
        {
            return await _abonnementDAO.FilterById(id);
        }

        public Task<Abonnement?> FindById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Abonnement>?> GetAll()
        {
            return await _abonnementDAO.GetAll();
        }

        public Task Update(Abonnement entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Abonnement>?> FindByStringId(string? id)
        {
            return await _abonnementDAO.FindByStringId(id);
        }
    }
}
