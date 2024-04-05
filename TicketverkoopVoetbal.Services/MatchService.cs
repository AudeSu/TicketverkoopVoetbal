using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketverkoopVoetbal.Domains.Entities;
using TicketverkoopVoetbal.Repositories.Interfaces;
using TicketverkoopVoetbal.Services.Interfaces;

namespace TicketverkoopVoetbal.Services
{
    public class MatchService : IService<Match>
    {
        private IDAO<Match> _matchDAO;

        public MatchService(IDAO<Match> matchDAO)
        {
            _matchDAO = matchDAO;
        }

        public async Task Add(Match entity)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(Match entity)
        {
            throw new NotImplementedException();
        }

        public async Task<Match> FindById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Match>?> GetAll()
        {
            return await _matchDAO.GetAll();
        }

        public async Task Update(Match entity)
        {
            throw new NotImplementedException();
        }
    }
}
