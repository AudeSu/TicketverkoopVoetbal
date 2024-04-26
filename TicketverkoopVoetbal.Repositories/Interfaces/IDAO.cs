namespace TicketverkoopVoetbal.Repositories.Interfaces
{
    public interface IDAO<T> where T : class
    {
        Task<IEnumerable<T>?> GetAll();
        Task<IEnumerable<T>?> FilterById(int id);
        Task Add(T entity);
        Task Delete(T entity);
        Task Update(T entity);
        Task<T?> FindById(int id);
    }
}
