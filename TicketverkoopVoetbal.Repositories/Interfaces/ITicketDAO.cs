using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketverkoopVoetbal.Repositories.Interfaces
{
    public interface ITicketDAO<T> : IDAO<T> where T : class
    {
        Task<IEnumerable<T>?> FindByStringId(string? id);

        Task<IEnumerable<T>?> FindPerUser(string? id, int matchID);
    }
}
