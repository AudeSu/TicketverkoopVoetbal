using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketverkoopVoetbal.Domains.Data;
using TicketverkoopVoetbal.Domains.Entities;
using TicketverkoopVoetbal.Repositories.Interfaces;

namespace TicketverkoopVoetbal.Repositories
{
    public class ZitplaatsDAO : IDAO<Zitplaat>
    {

        private readonly FootballDbContext _dbContext;

        public ZitplaatsDAO(FootballDbContext context)
        {
            _dbContext = context;
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
            try
            {
                return await _dbContext.Zitplaats
                    .Include(b => b.Zone)
                    .Include(b => b.Stadion)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in DAO");
                throw;
            }
        }

        public Task Update(Zitplaat entity)
        {
            throw new NotImplementedException();
        }
    }
}
