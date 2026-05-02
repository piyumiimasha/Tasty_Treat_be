using Microsoft.EntityFrameworkCore;
using Tasty_Treat_be.Data;
using Tasty_Treat_be.Interfaces.Repository;
using Tasty_Treat_be.Models;

namespace Tasty_Treat_be.Repositories
{
    public class ChatMsgRepository : Repository<ChatMsg>, IChatMsgRepository
    {
        public ChatMsgRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ChatMsg>> GetBySenderIdAsync(int senderId)
        {
            return await _dbSet
                .Include(m => m.Sender)
                .Where(m => m.SenderId == senderId)
                .OrderBy(m => m.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<ChatMsg>> GetUnreadMessagesAsync()
        {
            return await _dbSet
                .Include(m => m.Sender)
                .Where(m => !m.IsRead)
                .OrderBy(m => m.CreatedAt)
                .ToListAsync();
        }

        // All messages in a user's conversation with admin:
        // messages sent BY the user, OR admin replies TO the user
        public async Task<IEnumerable<ChatMsg>> GetConversationAsync(int userId)
        {
            return await _dbSet
                .Include(m => m.Sender)
                .Where(m => m.SenderId == userId || m.RecipientId == userId)
                .OrderBy(m => m.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<ChatMsg>> GetAllWithSendersAsync()
        {
            return await _dbSet
                .Include(m => m.Sender)
                .OrderBy(m => m.CreatedAt)
                .ToListAsync();
        }

        public async Task MarkMessagesReadAsync(int fromUserId)
        {
            var messages = await _dbSet
                .Where(m => m.SenderId == fromUserId && m.RecipientId == null && !m.IsRead)
                .ToListAsync();
            foreach (var m in messages) m.IsRead = true;
            await _context.SaveChangesAsync();
        }
    }
}
