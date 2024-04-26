namespace TicketverkoopVoetbal.Repositories.Interfaces
{
    public interface IUserDAO<T> : IDAO<T> where T : class
    {
        Task<T> FindByStringId(string id);
    }
}
