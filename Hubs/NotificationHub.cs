using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Tasty_Treat_be.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;
            var role = Context.User?.FindFirst(ClaimTypes.Role)?.Value;

            if (userId != null)
                await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");
            if (role != null)
                await Groups.AddToGroupAsync(Context.ConnectionId, $"role_{role}");

            await base.OnConnectedAsync();
        }
    }
}
