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
            return message != null ? _mapper.Map<ChatMsgDto>(message) : null;
        }

        public async Task<IEnumerable<ChatMsgDto>> GetAllAsync()
        {
            var messages = await _chatMsgRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ChatMsgDto>>(messages);
        }

        public async Task<IEnumerable<ChatMsgDto>> GetBySenderIdAsync(int senderId)
        {
            var messages = await _chatMsgRepository.GetBySenderIdAsync(senderId);
            return _mapper.Map<IEnumerable<ChatMsgDto>>(messages);
        }

        public async Task<IEnumerable<ChatMsgDto>> GetUnreadMessagesAsync()
        {
            var messages = await _chatMsgRepository.GetUnreadMessagesAsync();
            return _mapper.Map<IEnumerable<ChatMsgDto>>(messages);
        }

        public async Task<ChatMsgDto> CreateAsync(CreateChatMsgDto createChatMsgDto)
        {
            var message = _mapper.Map<ChatMsg>(createChatMsgDto);
            var createdMessage = await _chatMsgRepository.AddAsync(message);
            return _mapper.Map<ChatMsgDto>(createdMessage);
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

            var updatedMessage = await _chatMsgRepository.UpdateAsync(message);
            return _mapper.Map<ChatMsgDto>(updatedMessage);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _chatMsgRepository.DeleteAsync(id);
        }
    }
}
