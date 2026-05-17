using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Tasty_Treat_be.DTOs;
using Tasty_Treat_be.Hubs;
using Tasty_Treat_be.Interfaces.Repository;
using Tasty_Treat_be.Interfaces.Service;
using Tasty_Treat_be.Models;

namespace Tasty_Treat_be.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public NotificationService(
            INotificationRepository notificationRepository,
            IHubContext<NotificationHub> hubContext,
            IMapper mapper,
            IUserRepository userRepository)
        {
            _notificationRepository = notificationRepository;
            _hubContext = hubContext;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task NotifyUserAsync(int userId, string type, string message, int? referenceId = null)
        {
            var notification = new Notification
            {
                UserId = userId,
                Type = type,
                Message = message,
                ReferenceId = referenceId,
                CreatedAt = DateTime.UtcNow
            };

            await _notificationRepository.AddAsync(notification);

            var dto = _mapper.Map<NotificationDto>(notification);
            await _hubContext.Clients.Group($"user_{userId}")
                .SendAsync("ReceiveNotification", dto);
        }

        public async Task NotifyRoleAsync(string role, string type, string message, int? referenceId = null)
        {
            var users = await _userRepository.GetByRoleAsync(role);

            foreach (var user in users)
            {
                var notification = new Notification
                {
                    UserId = user.UserId,
                    Type = type,
                    Message = message,
                    ReferenceId = referenceId,
                    CreatedAt = DateTime.UtcNow
                };
                await _notificationRepository.AddAsync(notification);
            }

            var broadcastDto = new NotificationDto
            {
                Type = type,
                Message = message,
                ReferenceId = referenceId,
                CreatedAt = DateTime.UtcNow
            };
            await _hubContext.Clients.Group($"role_{role}")
                .SendAsync("ReceiveNotification", broadcastDto);
        }

        public async Task<IEnumerable<NotificationDto>> GetForUserAsync(int userId, int take = 50)
        {
            var notifications = await _notificationRepository.GetByUserIdAsync(userId, take);
            return _mapper.Map<IEnumerable<NotificationDto>>(notifications);
        }

        public async Task<int> GetUnreadCountAsync(int userId)
        {
            return await _notificationRepository.GetUnreadCountAsync(userId);
        }

        public async Task MarkReadAsync(int userId, List<int>? notificationIds)
        {
            if (notificationIds == null || notificationIds.Count == 0)
            {
                await _notificationRepository.MarkAllReadAsync(userId);
            }
            else
            {
                var notifications = await _notificationRepository.GetByUserIdAsync(userId, int.MaxValue);
                foreach (var n in notifications.Where(n => notificationIds.Contains(n.NotificationId)))
                {
                    n.IsRead = true;
                    await _notificationRepository.UpdateAsync(n);
                }
            }
        }
    }
}
