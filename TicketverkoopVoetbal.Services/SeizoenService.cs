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
    public class SeizoenService : ISeizoenService<Seizoen>
    {
        private ISeizoenDAO<Seizoen> _seizoenDAO;

        public SeizoenService(ISeizoenDAO<Seizoen> seizoenDAO)
        {
            _seizoenDAO = seizoenDAO;
        }

        Task IService<Seizoen>.Add(Seizoen entity)
        {
            throw new NotImplementedException();
        }

        Task IService<Seizoen>.Delete(Seizoen entity)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Seizoen>?> IService<Seizoen>.FilterById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Seizoen?> FindById(int id)
        {
            return await _seizoenDAO.FindById(id);
        }

        Task<IEnumerable<Seizoen>?> IService<Seizoen>.GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<Seizoen?> GetNextSeizoen()
        {
            return await _seizoenDAO.GetNextSeizoen();
        }

        Task IService<Seizoen>.Update(Seizoen entity)
        {
            throw new NotImplementedException();
        }
    }
}
