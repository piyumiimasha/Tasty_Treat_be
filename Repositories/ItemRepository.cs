using Microsoft.EntityFrameworkCore;
using Tasty_Treat_be.Data;
using Tasty_Treat_be.Interfaces.Repository;
using Tasty_Treat_be.Models;

namespace Tasty_Treat_be.Repositories
{
    public class ItemRepository : Repository<Item>, IItemRepository
    {
        public ItemRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<Item?> GetByIdAsync(int id)
        {
            return await _dbSet.Include(i => i.CategoryNav).FirstOrDefaultAsync(i => i.ItemId == id);
        }

        public override async Task<IEnumerable<Item>> GetAllAsync()
        {
            return await _dbSet.Include(i => i.CategoryNav).ToListAsync();
        }

        public async Task<IEnumerable<Item>> GetByCategoryAsync(string category)
        {
            return await _dbSet.Include(i => i.CategoryNav)
                .Where(i => i.CategoryNav != null && i.CategoryNav.Name == category)
                .ToListAsync();
        }
    }
}
