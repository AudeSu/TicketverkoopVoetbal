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
    public class ZoneService : IService<Zone>
    {
        private IDAO<Zone> _zoneDAO;

        public ZoneService(IDAO<Zone> zoneDAO)
        {
            _zoneDAO = zoneDAO;
        }

        public Task Add(Zone entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(Zone entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Zone>?> FilterById(int id)
        {
            return await _zoneDAO.FilterById(id);
        }

        public async Task<Zone?> FindById(int id)
        {
            return await _zoneDAO.FindById(id);
        }

        public async Task<IEnumerable<Zone>?> GetAll()
        {
            return await _zoneDAO.GetAll();
        }

        public Task Update(Zone entity)
        {
            throw new NotImplementedException();
        }
    }
}
