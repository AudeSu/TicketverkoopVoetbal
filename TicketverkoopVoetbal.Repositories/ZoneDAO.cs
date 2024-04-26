using Microsoft.EntityFrameworkCore;
using TicketverkoopVoetbal.Domains.Data;
using TicketverkoopVoetbal.Domains.Entities;
using TicketverkoopVoetbal.Repositories.Interfaces;

namespace TicketverkoopVoetbal.Repositories
{
    public class ZoneDAO : IDAO<Zone>
    {
        private readonly FootballDbContext _dbContext;

        public ZoneDAO(FootballDbContext context)
        {
            _dbContext = context;
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
            try
            {
                return await _dbContext.Zones
                 .Where(a => a.StadionId == id)
                 .Include(b => b.Stadion)
                 .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in DAO");
                throw;
            }
        }

        public async Task<Zone?> FindById(int id)
        {
            try
            {
                return await _dbContext.Zones
                .Where(a => a.ZoneId == id)
                 .Include(b => b.Stadion)

                .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in DAO");
                throw;
            }
        }

        public async Task<IEnumerable<Zone>?> GetAll()
        {
            try
            {
                return await _dbContext.Zones
                    .Include(b => b.Stadion)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in DAO");
                throw;
            }
        }

        public Task Update(Zone entity)
        {
            throw new NotImplementedException();
        }
    }
}
