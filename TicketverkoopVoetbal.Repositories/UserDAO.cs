using Microsoft.EntityFrameworkCore;
using TicketverkoopVoetbal.Domains.Data;
using TicketverkoopVoetbal.Domains.Entities;
using TicketverkoopVoetbal.Repositories.Interfaces;

namespace TicketverkoopVoetbal.Repositories
{
    public class UserDAO : IUserDAO<AspNetUser>
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

        public async Task<AspNetUser> FindByStringId(string id)
        {
            try
            {
                return await _dbContext.AspNetUsers.Where(u => u.Id == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<IEnumerable<AspNetUser>?> GetAll()
        {
            try
            {
                return await _dbContext.AspNetUsers.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
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
