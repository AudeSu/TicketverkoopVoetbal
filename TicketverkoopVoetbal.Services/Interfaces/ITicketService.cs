using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketverkoopVoetbal.Services.Interfaces
{
    public interface ITicketService<T> : IService<T> where T : class
    {
        Task<IEnumerable<T>?> FindByStringId(string? id);

        Task<IEnumerable<T>?> FindPerUser(string? id, int matchID);
    }
}
