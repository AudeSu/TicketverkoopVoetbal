using TicketverkoopVoetbal.Domains.Entities;
using TicketverkoopVoetbal.Repositories.Interfaces;
using TicketverkoopVoetbal.Services.Interfaces;

namespace TicketverkoopVoetbal.Services
{
    public class MatchService : IMatchService<Match>
    {
        private IMatchDAO<Match> _matchDAO;

        public MatchService(IMatchDAO<Match> matchDAO)
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

        public async Task<Match?> FindById(int id)
        {
            return await _matchDAO.FindById(id);
        }

        public async Task<IEnumerable<Match>?> FilterById(int id)
        {
            return await _matchDAO.FilterById(id);
        }

        public async Task<IEnumerable<Match>?> GetAll()
        {
            return await _matchDAO.GetAll();
        }

        public async Task Update(Match entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Match>?> FindByTwoIds(int thuisploedId, int uitploegId)
        {
            return await _matchDAO.FindByTwoIds(thuisploedId, uitploegId);
        }

        public async Task<IEnumerable<Match>?> FindByHomeClub(int thuisploegId)
        {
            return await _matchDAO.FindByHomeClub(thuisploegId);
        }

        public async Task<IEnumerable<Match>?> GetFutureMatches()
        {
            return await _matchDAO.GetFutureMatches();
        }
    }
}
