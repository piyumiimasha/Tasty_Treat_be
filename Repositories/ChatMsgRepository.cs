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

        // Admin↔user support conversation:
        // only messages the user sent TO admin (recipientId == null), plus admin replies back to the user
        public async Task<IEnumerable<ChatMsg>> GetConversationAsync(int userId)
        {
            return await _dbSet
                .Include(m => m.Sender)
                .Where(m =>
                    (m.SenderId == userId && m.RecipientId == null) ||
                    (m.RecipientId == userId && (m.Sender == null || m.Sender.Role != "DeliveryPerson")))
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

        // All messages sent directly TO userId (non-admin channel, i.e. recipientId == userId)
        public async Task<IEnumerable<ChatMsg>> GetDirectMessagesReceivedAsync(int userId)
        {
            return await _dbSet
                .Include(m => m.Sender)
                .Where(m => m.RecipientId == userId && m.Sender != null)
                .OrderBy(m => m.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<ChatMsg>> GetDirectMessagesAsync(int user1Id, int user2Id)
        {
            return await _dbSet
                .Include(m => m.Sender)
                .Where(m =>
                    (m.SenderId == user1Id && m.RecipientId == user2Id) ||
                    (m.SenderId == user2Id && m.RecipientId == user1Id))
                .OrderBy(m => m.CreatedAt)
                .ToListAsync();
        }

        public async Task MarkDirectMessagesReadAsync(int fromUserId, int toUserId)
        {
            var messages = await _dbSet
                .Where(m => m.SenderId == fromUserId && m.RecipientId == toUserId && !m.IsRead)
                .ToListAsync();
            foreach (var m in messages) m.IsRead = true;
            await _context.SaveChangesAsync();
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
