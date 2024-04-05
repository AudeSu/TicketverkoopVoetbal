using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketverkoopVoetbal.Domains.Entities;
using TicketverkoopVoetbal.Repositories;
using TicketverkoopVoetbal.Repositories.Interfaces;
using TicketverkoopVoetbal.Services.Interfaces;

namespace TicketverkoopVoetbal.Services
{
    public class UserService : IService<AspNetUser>
    {
        private IDAO<AspNetUser> _userDAO;

        public UserService(IDAO<AspNetUser> userDAO)
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

        public Task<AspNetUser> FindById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AspNetUser>?> GetAll()
        {
            return await _userDAO.GetAll();
        }

        public Task Update(AspNetUser entity)
        {
            throw new NotImplementedException();
        }
    }
}
