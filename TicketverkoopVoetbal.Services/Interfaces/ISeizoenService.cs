using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketverkoopVoetbal.Services.Interfaces
{
    public interface ISeizoenService<T> : IService<T> where T : class
    {
        Task<T?> GetNextSeizoen();
    }
}
