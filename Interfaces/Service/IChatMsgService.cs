using Tasty_Treat_be.DTOs;

namespace Tasty_Treat_be.Interfaces.Service
{
    public interface IChatMsgService
    {
        Task<ChatMsgDto?> GetByIdAsync(int id);
        Task<IEnumerable<ChatMsgDto>> GetAllAsync();
        Task<IEnumerable<ChatMsgDto>> GetBySenderIdAsync(int senderId);
        Task<IEnumerable<ChatMsgDto>> GetUnreadMessagesAsync();
        Task<ChatMsgDto> CreateAsync(CreateChatMsgDto createChatMsgDto);
        Task<ChatMsgDto> UpdateAsync(int id, UpdateChatMsgDto updateChatMsgDto);
        Task<bool> DeleteAsync(int id);
    }
}
