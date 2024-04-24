namespace TicketverkoopVoetbal.Services.Interfaces
{
    public interface IUserService<T> : IService<T> where T : class
    {
        Task<T> FindByStringId(string id);
    }
}
