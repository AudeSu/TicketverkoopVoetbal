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
    public class SeizoenDAO : ISeizoenDAO<Seizoen>
    {

        private readonly FootballDbContext _dbContext;

        public SeizoenDAO(FootballDbContext context)
        {
            _dbContext = context;
        }

        Task IDAO<Seizoen>.Add(Seizoen entity)
        {
            throw new NotImplementedException();
        }

        Task IDAO<Seizoen>.Delete(Seizoen entity)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Seizoen>?> IDAO<Seizoen>.FilterById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Seizoen?> FindById(int id)
        {
            try
            {

                return await _dbContext.Seizoens
                    .Where(m => m.SeizoenId == id)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        Task<IEnumerable<Seizoen>?> IDAO<Seizoen>.GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<Seizoen?> GetNextSeizoen()
        {

            try
            {

                return await _dbContext.Seizoens
                    .Where(m => m.Startdatum >= DateTime.Now)
                    .OrderBy(m => m.Startdatum)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        Task IDAO<Seizoen>.Update(Seizoen entity)
        {

            throw new NotImplementedException();
        }
    }
}
