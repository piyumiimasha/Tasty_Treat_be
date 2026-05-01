using Microsoft.EntityFrameworkCore;
using Tasty_Treat_be.Data;
using Tasty_Treat_be.Interfaces.Repository;
using Tasty_Treat_be.Models;

namespace Tasty_Treat_be.Repositories
{
    public class ItemFlavourRepository : Repository<ItemFlavour>, IItemFlavourRepository
    {
        public ItemFlavourRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ItemFlavour>> GetByItemIdAsync(int itemId)
        {
            return await _dbSet.Where(f => f.ItemId == itemId).ToListAsync();
        }
    }
}
