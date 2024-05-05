namespace TicketverkoopVoetbal.Services.Interfaces
{
    public interface IMatchService<T> : IService<T> where T : class
    {
        Task<IEnumerable<T>?> FindByTwoIds(int thuisploedId, int uitploegId);
    }
}
