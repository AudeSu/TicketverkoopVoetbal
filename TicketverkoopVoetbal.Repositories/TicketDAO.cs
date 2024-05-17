using Microsoft.EntityFrameworkCore;
using TicketverkoopVoetbal.Domains.Data;
using TicketverkoopVoetbal.Domains.Entities;
using TicketverkoopVoetbal.Repositories.Interfaces;

namespace TicketverkoopVoetbal.Repositories
{
    public class TicketDAO : ITicketDAO<Ticket>
    {
        private readonly FootballDbContext _dbContext;

        public TicketDAO(FootballDbContext context)
        {
            _dbContext = context;
        }

        public async Task Add(Ticket entity)
        {
            _dbContext.Entry(entity).State = EntityState.Added;
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                Console.WriteLine("Error in DAO");
                throw;
            }
        }

        public async Task Delete(Ticket entity)
        {
            _dbContext.Entry(entity).State = EntityState.Deleted;
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                Console.WriteLine("Error in DAO");
                throw;
            }
        }

        public Task<IEnumerable<Ticket>?> FilterById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Ticket?> FindById(int id)
        {
            try
            {
                return await _dbContext.Tickets
                    .Where(a => a.TicketId == id)
                    .Include(a => a.Gebruikers)
                    .Include(a => a.Match)
                    .Include(b => b.Stoeltje)
                    .FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                Console.WriteLine("Error in DAO");
                throw;
            }
        }

        public Task<IEnumerable<Ticket>?> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task Update(Ticket entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Ticket>?> FindByStringId(string? id)
        {
            try
            {
                return await _dbContext.Tickets
                    .Where(a => a.GebruikersId == id)
                    .Include(a => a.Gebruikers)
                    .Include(a => a.Match)
                    .Include(b => b.Stoeltje)
                    .ToListAsync();
            }
            catch (Exception)
            {
                Console.WriteLine("Error in DAO");
                throw;
            }
        }

        public async Task<IEnumerable<Ticket>?>FindPerUser(string? id, int matchID)
        {
            try
            {
                return await _dbContext.Tickets
                    .Where(a => a.GebruikersId == id && a.MatchId == matchID)
                    .Include(a => a.Gebruikers)
                    .Include(a => a.Match)
                    .Include(b => b.Stoeltje)
                    .ToListAsync();
            }
            catch (Exception)
            {
                Console.WriteLine("Error in DAO");
                throw;
            }
        }
    }
}
