using Tasty_Treat_be.DTOs;

namespace Tasty_Treat_be.Interfaces.Service
{
    public interface IChatMsgService
    {
        Task<ChatMsgDto?> GetByIdAsync(int id);
        Task<IEnumerable<ChatMsgDto>> GetAllAsync();
        Task<IEnumerable<ChatMsgDto>> GetBySenderIdAsync(int senderId);
        Task<IEnumerable<ChatMsgDto>> GetUnreadMessagesAsync();
        Task<IEnumerable<ChatMsgDto>> GetConversationAsync(int userId);
        Task<IEnumerable<ConversationUserDto>> GetConversationUsersAsync();
        Task<ChatMsgDto> CreateAsync(CreateChatMsgDto createChatMsgDto);
        Task<ChatMsgDto> UpdateAsync(int id, UpdateChatMsgDto updateChatMsgDto);
        Task<bool> DeleteAsync(int id);
        Task MarkMessagesReadAsync(int fromUserId);
        Task<IEnumerable<ChatMsgDto>> GetDirectConversationAsync(int user1Id, int user2Id);
        Task<IEnumerable<ConversationUserDto>> GetDirectPartnersAsync(int userId);
        Task MarkDirectMessagesReadAsync(int fromUserId, int toUserId);
    }
}
