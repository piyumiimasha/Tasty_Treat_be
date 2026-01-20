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
            return await _dbSet.Where(m => m.SenderId == senderId).ToListAsync();
        }

        public async Task<IEnumerable<ChatMsg>> GetUnreadMessagesAsync()
        {
            return await _dbSet.Where(m => !m.IsRead).ToListAsync();
        }
    }
}
