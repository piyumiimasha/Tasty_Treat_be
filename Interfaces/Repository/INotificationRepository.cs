using Tasty_Treat_be.Models;

namespace Tasty_Treat_be.Interfaces.Repository
{
    public interface INotificationRepository : IRepository<Notification>
    {
        Task<IEnumerable<Notification>> GetByUserIdAsync(int userId, int take = 50);
        Task<int> GetUnreadCountAsync(int userId);
        Task MarkAllReadAsync(int userId);
    }
}
