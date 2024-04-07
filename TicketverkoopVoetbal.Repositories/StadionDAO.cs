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
    public class StadionDAO : IDAO<Stadion>
    {
        private readonly FootballDbContext _dbContext;

        public StadionDAO(FootballDbContext context)
        {
            _dbContext = context;
        }

        public Task Add(Stadion entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(Stadion entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Stadion>?> FindById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Stadion>?> GetAll()
        {
            try
            {
                return await _dbContext.Stadions
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in DAO");
                throw;
            }
        }

        public Task Update(Stadion entity)
        {
            throw new NotImplementedException();
        }
    }
}
