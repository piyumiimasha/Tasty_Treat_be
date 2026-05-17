using Tasty_Treat_be.Models;

namespace Tasty_Treat_be.Interfaces.Repository
{
    public interface IChatMsgRepository : IRepository<ChatMsg>
    {
        Task<IEnumerable<ChatMsg>> GetBySenderIdAsync(int senderId);
        Task<IEnumerable<ChatMsg>> GetUnreadMessagesAsync();
        Task<IEnumerable<ChatMsg>> GetConversationAsync(int userId);
        Task<IEnumerable<ChatMsg>> GetAllWithSendersAsync();
        Task MarkMessagesReadAsync(int fromUserId);
        Task<IEnumerable<ChatMsg>> GetDirectMessagesAsync(int user1Id, int user2Id);
        Task<IEnumerable<ChatMsg>> GetDirectMessagesReceivedAsync(int userId);
        Task MarkDirectMessagesReadAsync(int fromUserId, int toUserId);
    }
}
