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
    public class MatchDAO : IDAO<Match>
    {
        private readonly FootballDbContext _dbContext;

        public MatchDAO(FootballDbContext context)
        {
            _dbContext = context;
        }

        public async Task Add(Match entity)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(Match entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Match>?> FindById(int id)
        {
            try
            {
                return await _dbContext.Matches
                .Where(a => a.ThuisploegId == id || a.UitploegId == id)
                .Include(a => a.Thuisploeg)
                .Include(a => a.Uitploeg)
                 .Include(b => b.Stadion)

                .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in DAO");
                throw;
            }
        }

        public async Task<IEnumerable<Match>?> GetAll()
        {
            try
            {
                return await _dbContext.Matches
                    .Include(b => b.Thuisploeg)
                    .Include(b=> b.Uitploeg)
                    .Include(b => b.Stadion)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in DAO");
                throw;
            }
        }

        public async Task Update(Match entity)
        {
            throw new NotImplementedException();
        }
    }
}
