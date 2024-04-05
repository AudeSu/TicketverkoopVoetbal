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
    public class ClubDAO : IDAO<Club>
    {
        private readonly FootballDbContext _dbContext;

        public ClubDAO(FootballDbContext context)
        {
            _dbContext = context;
        }

        public Task Add(Club entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(Club entity)
        {
            throw new NotImplementedException();
        }

        public Task<Club> FindById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Club>?> GetAll()
        {
            try
            {
                return await _dbContext.Clubs
                    .Include(b => b.Thuisstadion)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in DAO");
                throw;
            }
        }

        public Task Update(Club entity)
        {
            throw new NotImplementedException();
        }
    }
}
