using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketverkoopVoetbal.Services.Interfaces
{
    public interface IService<T> where T : class
    {
        Task<IEnumerable<T>?> GetAll();

        Task<IEnumerable<T>?> FilterById(int id);
        Task Add(T entity);
        Task Delete(T entity);
        Task Update(T entity);
        Task<T?> FindById(int id);
    }
}
