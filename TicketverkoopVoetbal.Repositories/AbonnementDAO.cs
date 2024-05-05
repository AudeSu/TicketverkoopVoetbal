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
    public class AbonnementDAO : IDAO<Abonnement>
    {
        private readonly FootballDbContext _dbContext;

        public AbonnementDAO(FootballDbContext context)
        {
            _dbContext = context;
        }

        public async Task Add(Abonnement entity)
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

        public Task Delete(Abonnement entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Abonnement>?> FilterById(int id)
        {
            try
            {
                return await _dbContext.Abonnements
                .Where(a => a.Club.ClubId == id)
                .Include(a => a.Club)
                .Include(a => a.Gebruiker)
                 .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in DAO");
                throw;
            }
        }

        public Task<Abonnement?> FindById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Abonnement>?> GetAll()
        {
            try
            {
                return await _dbContext.Abonnements
                    .Include(b => b.Club)
                    .Include(b => b.Gebruiker)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in DAO");
                throw;
            }
        }

        public Task Update(Abonnement entity)
        {
            throw new NotImplementedException();
        }
    }
}
