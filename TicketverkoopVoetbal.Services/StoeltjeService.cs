using TicketverkoopVoetbal.Domains.Entities;
using TicketverkoopVoetbal.Repositories;
using TicketverkoopVoetbal.Repositories.Interfaces;
using TicketverkoopVoetbal.Services.Interfaces;

namespace TicketverkoopVoetbal.Services
{
    public class StoeltjeService : IStoelService<Stoeltje>
    {
        private IStoelDAO<Stoeltje> _stoeltjeDAO;

        public StoeltjeService(IStoelDAO<Stoeltje> stoeltjeDAO)
        {
            _stoeltjeDAO = stoeltjeDAO;
        }

        public async Task Add(Stoeltje entity)
        {
            await _stoeltjeDAO.Add(entity);
        }

        public Task Delete(Stoeltje entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Stoeltje>?> FilterById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Stoeltje?> FindById(int id)
        {
            return await _stoeltjeDAO.FindById(id);
        }

        public async Task<IEnumerable<Stoeltje>?> GetAll()
        {
            return await _stoeltjeDAO.GetAll();
        }

        public async Task Update(Stoeltje entity)
        {
            await _stoeltjeDAO.Update(entity);
        }

        public async Task<IEnumerable<Stoeltje>>GetTakenSeatsByClubID(int ClubID, int ZoneID, int SeizoenID)
        {
            return await _stoeltjeDAO.GetTakenSeatsByClubID(ClubID,ZoneID,SeizoenID);
        }

        public async Task<IEnumerable<Stoeltje>>GetTakenSeatsByMatchID(int MatchID, int ZoneID)
        {
            return await _stoeltjeDAO.GetTakenSeatsByMatchID(MatchID,ZoneID);
        }

        public async Task<Stoeltje?> GetEmptySeat(int MatchID, int ZoneID)
        {
            return await _stoeltjeDAO.GetEmptySeat(MatchID, ZoneID);
        }
    }
}
