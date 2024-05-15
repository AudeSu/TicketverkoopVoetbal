using Microsoft.EntityFrameworkCore;
using TicketverkoopVoetbal.Domains.Data;
using TicketverkoopVoetbal.Domains.Entities;
using TicketverkoopVoetbal.Repositories.Interfaces;

namespace TicketverkoopVoetbal.Repositories
{
    public class StoeltjeDAO : IStoelDAO<Stoeltje>
    {

        private readonly FootballDbContext _dbContext;

        public StoeltjeDAO(FootballDbContext context)
        {
            _dbContext = context;
        }
        public async Task Add(Stoeltje entity)
        {
            _dbContext.Entry(entity).State = EntityState.Added;
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public Task Delete(Stoeltje entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Stoeltje>?> FilterById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Stoeltje?> FindById(int id)
        {
            try
            {
                return await _dbContext.Stoeltjes
                .Where(a => a.StoeltjeId == id)
                .Include(a => a.Zone)
                .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in DAO");
                throw;
            }
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

        public async Task Update(Stoeltje entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<IEnumerable<Stoeltje>>GetTakenSeatsByClubID(int ClubID, int ZoneID, int SeizoenID)
        {
            try
            {

                return await _dbContext.Stoeltjes
                    .Where(b => b.ZoneId == ZoneID && b.ClubId== ClubID && b.MatchId == null)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<IEnumerable<Stoeltje>>GetTakenSeatsByMatchID(int MatchID, int ZoneID)
        {
            try
            {

                return await _dbContext.Stoeltjes
                    .Where(b => b.ZoneId == ZoneID &&  b.MatchId == MatchID)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<Stoeltje?>GetEmptySeat(int MatchID, int ZoneID)
        {
            try
            {

                return await _dbContext.Stoeltjes
                    .Where(b => b.ZoneId == ZoneID && b.MatchId == MatchID && b.Bezet == false)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
