using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tasty_Treat_be.DTOs;
using Tasty_Treat_be.Interfaces.Service;

namespace Tasty_Treat_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<NotificationDto>>> GetForUser(int userId, [FromQuery] int take = 50)
        {
            try
            {
                var notifications = await _notificationService.GetForUserAsync(userId, take);
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("user/{userId}/unread-count")]
        public async Task<ActionResult<int>> GetUnreadCount(int userId)
        {
            try
            {
                var count = await _notificationService.GetUnreadCountAsync(userId);
                return Ok(count);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("user/{userId}/mark-read")]
        public async Task<ActionResult> MarkRead(int userId, [FromBody] MarkReadDto dto)
        {
            try
            {
                await _notificationService.MarkReadAsync(userId, dto.NotificationIds);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("notify-role")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> NotifyRole([FromBody] NotifyRoleDto dto)
        {
            try
            {
                await _notificationService.NotifyRoleAsync(dto.Role, dto.Type, dto.Message, dto.ReferenceId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
