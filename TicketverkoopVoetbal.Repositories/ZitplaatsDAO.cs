using Microsoft.EntityFrameworkCore;
using TicketverkoopVoetbal.Domains.Data;
using TicketverkoopVoetbal.Domains.Entities;
using TicketverkoopVoetbal.Repositories.Interfaces;

namespace TicketverkoopVoetbal.Repositories
{
    public class ZitplaatsDAO : IDAO<Zitplaats>
    {

        private readonly FootballDbContext _dbContext;

        public ZitplaatsDAO(FootballDbContext context)
        {
            _dbContext = context;
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
            try
            {
                return await _dbContext.Zitplaats
                    .Include(b => b.Zone)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in DAO");
                throw;
            }
        }

        public Task Update(Zitplaats entity)
        {
            throw new NotImplementedException();
        }
    }
}
