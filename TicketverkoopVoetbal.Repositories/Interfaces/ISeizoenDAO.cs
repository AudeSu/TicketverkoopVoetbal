using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketverkoopVoetbal.Repositories.Interfaces
{
    public interface ISeizoenDAO<T> : IDAO<T> where T : class
    {
        Task<T?> GetNextSeizoen();
    }
}
