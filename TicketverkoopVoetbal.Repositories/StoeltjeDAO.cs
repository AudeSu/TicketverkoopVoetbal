using Microsoft.EntityFrameworkCore;
using TicketverkoopVoetbal.Domains.Data;
using TicketverkoopVoetbal.Domains.Entities;
using TicketverkoopVoetbal.Repositories.Interfaces;

namespace TicketverkoopVoetbal.Repositories
{
    public class StoeltjeDAO : IDAO<Stoeltje>
    {

        private readonly FootballDbContext _dbContext;

        public StoeltjeDAO(FootballDbContext context)
        {
            _dbContext = context;
        }
        public Task Add(Stoeltje entity)
        {
            throw new NotImplementedException();
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
            try
            {
                return await _dbContext.Stoeltjes
                    .Include(b => b.Zone)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in DAO");
                throw;
            }
        }

        public Task Update(Stoeltje entity)
        {
            throw new NotImplementedException();
        }
    }
}
