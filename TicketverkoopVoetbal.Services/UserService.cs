using TicketverkoopVoetbal.Domains.Entities;
using TicketverkoopVoetbal.Repositories.Interfaces;
using TicketverkoopVoetbal.Services.Interfaces;

namespace TicketverkoopVoetbal.Services
{
    public class UserService : IUserService<AspNetUser>
    {
        private IUserDAO<AspNetUser> _userDAO;

        public UserService(IUserDAO<AspNetUser> userDAO)
        {
            _userDAO = userDAO;
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
            return await _userDAO.FindByStringId(id);
        }

        public async Task<IEnumerable<AspNetUser>?> GetAll()
        {
            return await _userDAO.GetAll();
        }

        public Task Update(AspNetUser entity)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<AspNetUser>?> IService<AspNetUser>.FilterById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
