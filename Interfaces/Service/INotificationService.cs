using Tasty_Treat_be.DTOs;

namespace Tasty_Treat_be.Interfaces.Service
{
    public interface INotificationService
    {
        Task NotifyUserAsync(int userId, string type, string message, int? referenceId = null);
        Task NotifyRoleAsync(string role, string type, string message, int? referenceId = null);
        Task<IEnumerable<NotificationDto>> GetForUserAsync(int userId, int take = 50);
        Task<int> GetUnreadCountAsync(int userId);
        Task MarkReadAsync(int userId, List<int>? notificationIds);
    }
}
