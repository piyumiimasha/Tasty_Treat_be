using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tasty_Treat_be.DTOs;
using Tasty_Treat_be.Interfaces.Service;

namespace Tasty_Treat_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatMessagesController : ControllerBase
    {
        private readonly IChatMsgService _chatMsgService;

        public ChatMessagesController(IChatMsgService chatMsgService)
        {
            _chatMsgService = chatMsgService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<ChatMsgDto>>> GetAll()
        {
            var messages = await _chatMsgService.GetAllAsync();
            return Ok(messages);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ChatMsgDto>> GetById(int id)
        {
            var message = await _chatMsgService.GetByIdAsync(id);
            if (message == null)
                return NotFound($"Message with id {id} not found");

            return Ok(message);
        }

        [HttpGet("sender/{senderId}")]
        public async Task<ActionResult<IEnumerable<ChatMsgDto>>> GetBySenderId(int senderId)
        {
            var messages = await _chatMsgService.GetBySenderIdAsync(senderId);
            return Ok(messages);
        }

        [HttpGet("unread")]
        public async Task<ActionResult<IEnumerable<ChatMsgDto>>> GetUnread()
        {
            var messages = await _chatMsgService.GetUnreadMessagesAsync();
            return Ok(messages);
        }

        // All messages between user {userId} and admin
        [HttpGet("conversation/{userId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ChatMsgDto>>> GetConversation(int userId)
        {
            var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("nameid")?.Value;

            if (!User.IsInRole("Admin") &&
                (!int.TryParse(currentUserIdClaim, out var currentUserId) || currentUserId != userId))
            {
                return Forbid();
            }

            var messages = await _chatMsgService.GetConversationAsync(userId);
            return Ok(messages);
        }

        // Distinct customer list for admin sidebar
        [HttpGet("conversation-users")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<ConversationUserDto>>> GetConversationUsers()
        {
            var users = await _chatMsgService.GetConversationUsersAsync();
            return Ok(users);
        }

        // Users who have sent direct messages to userId (shows in customer's sidebar)
        [HttpGet("direct-partners/{userId}")]
        public async Task<ActionResult<IEnumerable<ConversationUserDto>>> GetDirectPartners(int userId)
        {
            var partners = await _chatMsgService.GetDirectPartnersAsync(userId);
            return Ok(partners);
        }

        // Direct messages between two specific users (e.g. delivery person ↔ customer)
        [HttpGet("direct/{user1Id}/{user2Id}")]
        public async Task<ActionResult<IEnumerable<ChatMsgDto>>> GetDirectConversation(int user1Id, int user2Id)
        {
            var messages = await _chatMsgService.GetDirectConversationAsync(user1Id, user2Id);
            return Ok(messages);
        }

        [HttpPut("mark-read/{fromUserId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> MarkRead(int fromUserId)
        {
            await _chatMsgService.MarkMessagesReadAsync(fromUserId);
            return NoContent();
        }

        [HttpPut("direct-mark-read/{fromUserId}/{toUserId}")]
        public async Task<ActionResult> MarkDirectRead(int fromUserId, int toUserId)
        {
            await _chatMsgService.MarkDirectMessagesReadAsync(fromUserId, toUserId);
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<ChatMsgDto>> Create([FromBody] CreateChatMsgDto createChatMsgDto)
        {
            try
            {
                var message = await _chatMsgService.CreateAsync(createChatMsgDto);
                return CreatedAtAction(nameof(GetById), new { id = message.MsgId }, message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ChatMsgDto>> Update(int id, [FromBody] UpdateChatMsgDto updateChatMsgDto)
        {
            try
            {
                var message = await _chatMsgService.UpdateAsync(id, updateChatMsgDto);
                return Ok(message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _chatMsgService.DeleteAsync(id);
            if (!result)
                return NotFound($"Message with id {id} not found");

            return NoContent();
        }
    }
}
