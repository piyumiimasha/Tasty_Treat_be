using AutoMapper;
using Tasty_Treat_be.DTOs;
using Tasty_Treat_be.Interfaces.Repository;
using Tasty_Treat_be.Interfaces.Service;
using Tasty_Treat_be.Models;

namespace Tasty_Treat_be.Services
{
    public class ChatMsgService : IChatMsgService
    {
        private readonly IChatMsgRepository _chatMsgRepository;
        private readonly IMapper _mapper;

        public ChatMsgService(IChatMsgRepository chatMsgRepository, IMapper mapper)
        {
            _chatMsgRepository = chatMsgRepository;
            _mapper = mapper;
        }

        public async Task<ChatMsgDto?> GetByIdAsync(int id)
        {
            var message = await _chatMsgRepository.GetByIdAsync(id);
            return message != null ? MapToDto(message) : null;
        }

        public async Task<IEnumerable<ChatMsgDto>> GetAllAsync()
        {
            var messages = await _chatMsgRepository.GetAllWithSendersAsync();
            return messages.Select(MapToDto);
        }

        public async Task<IEnumerable<ChatMsgDto>> GetBySenderIdAsync(int senderId)
        {
            var messages = await _chatMsgRepository.GetBySenderIdAsync(senderId);
            return messages.Select(MapToDto);
        }

        public async Task<IEnumerable<ChatMsgDto>> GetUnreadMessagesAsync()
        {
            var messages = await _chatMsgRepository.GetUnreadMessagesAsync();
            return messages.Select(MapToDto);
        }

        public async Task<IEnumerable<ChatMsgDto>> GetConversationAsync(int userId)
        {
            var messages = await _chatMsgRepository.GetConversationAsync(userId);
            return messages.Select(MapToDto);
        }

        // Returns one entry per customer who has ever sent a message to admin (RecipientId == null)
        public async Task<IEnumerable<ConversationUserDto>> GetConversationUsersAsync()
        {
            var all = await _chatMsgRepository.GetAllWithSendersAsync();

            // Customer messages are those sent with RecipientId == null (to admin)
            var customerMessages = all.Where(m => m.RecipientId == null && m.Sender != null).ToList();

            var grouped = customerMessages
                .GroupBy(m => m.SenderId)
                .Select(g =>
                {
                    var last = g.OrderByDescending(m => m.CreatedAt).First();
                    return new ConversationUserDto
                    {
                        UserId = g.Key,
                        Name = last.Sender?.Name ?? "Unknown",
                        LastMessage = last.MsgTxt,
                        LastMessageAt = last.CreatedAt,
                        UnreadCount = g.Count(m => !m.IsRead),
                    };
                })
                .OrderByDescending(u => u.LastMessageAt)
                .ToList();

            return grouped;
        }

        public async Task<ChatMsgDto> CreateAsync(CreateChatMsgDto createChatMsgDto)
        {
            var message = new ChatMsg
            {
                SenderId = createChatMsgDto.SenderId,
                RecipientId = createChatMsgDto.RecipientId,
                MsgTxt = createChatMsgDto.MsgTxt,
                IsRead = false,
                CreatedAt = DateTime.UtcNow,
            };
            var created = await _chatMsgRepository.AddAsync(message);

            // Re-fetch with Sender navigation loaded so SenderName is populated
            var messages = await _chatMsgRepository.GetConversationAsync(createChatMsgDto.SenderId);
            var withSender = messages.FirstOrDefault(m => m.MsgId == created.MsgId);
            return MapToDto(withSender ?? created);
        }

        public async Task<ChatMsgDto> UpdateAsync(int id, UpdateChatMsgDto updateChatMsgDto)
        {
            var message = await _chatMsgRepository.GetByIdAsync(id);
            if (message == null)
                throw new KeyNotFoundException($"ChatMsg with id {id} not found");

            if (!string.IsNullOrEmpty(updateChatMsgDto.MsgTxt))
                message.MsgTxt = updateChatMsgDto.MsgTxt;
            if (updateChatMsgDto.IsRead.HasValue)
                message.IsRead = updateChatMsgDto.IsRead.Value;

            var updated = await _chatMsgRepository.UpdateAsync(message);
            return MapToDto(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _chatMsgRepository.DeleteAsync(id);
        }

        public async Task MarkMessagesReadAsync(int fromUserId)
        {
            await _chatMsgRepository.MarkMessagesReadAsync(fromUserId);
        }

        private static ChatMsgDto MapToDto(ChatMsg m) => new()
        {
            MsgId = m.MsgId,
            SenderId = m.SenderId,
            SenderName = m.Sender?.Name ?? string.Empty,
            RecipientId = m.RecipientId,
            MsgTxt = m.MsgTxt,
            IsRead = m.IsRead,
            CreatedAt = m.CreatedAt,
        };
    }
}
