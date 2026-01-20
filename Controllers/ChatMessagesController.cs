using Microsoft.AspNetCore.Mvc;
using Tasty_Treat_be.DTOs;
using Tasty_Treat_be.Interfaces.Service;

namespace Tasty_Treat_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatMessagesController : ControllerBase
    {
        private readonly IChatMsgService _chatMsgService;

        public ChatMessagesController(IChatMsgService chatMsgService)
        {
            _chatMsgService = chatMsgService;
        }

        [HttpGet]
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
