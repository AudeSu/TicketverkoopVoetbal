namespace TicketverkoopVoetbal.Repositories.Interfaces
{
    public interface IMatchDAO<T> : IDAO<T> where T : class
    {
        Task<IEnumerable<T>?> FindByTwoIds(int thuisploegId, int uitploegId);
        Task<IEnumerable<T>?> FindByHomeClub(int thuisploegId);
        Task<IEnumerable<T>?> GetFutureMatches();
    }
}
