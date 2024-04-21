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
    public class UserDAO : IDAO<AspNetUser>
    {
        private readonly FootballDbContext _dbContext;

        public UserDAO(FootballDbContext context)
        {
            _dbContext = context;
        }

        public Task Add(AspNetUser entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(AspNetUser entity)
        {
            throw new NotImplementedException();
        }

        public Task<AspNetUser?> FindById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AspNetUser>?> GetAll()
        {
            try
            {
                return await _dbContext.AspNetUsers.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in DAO");
                throw;
            }
        }

        public Task Update(AspNetUser entity)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<AspNetUser>?> IDAO<AspNetUser>.FilterById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
