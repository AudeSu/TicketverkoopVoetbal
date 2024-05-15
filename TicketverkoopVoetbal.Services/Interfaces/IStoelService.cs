using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketverkoopVoetbal.Domains;

namespace TicketverkoopVoetbal.Services.Interfaces
{
    public interface IStoelService<T> : IService<T> where T : class
    {

        Task<IEnumerable<T>> GetTakenSeatsByMatchID(int MatchID, int ZoneID);
        Task<IEnumerable<T>> GetTakenSeatsByClubID(int ClubID, int ZoneID, int SeizoenID);

        Task<T?> GetEmptySeat(int MatchID, int ZoneID);
    }
}
