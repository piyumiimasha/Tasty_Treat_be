using Microsoft.EntityFrameworkCore;
using Tasty_Treat_be.Data;
using Tasty_Treat_be.Interfaces.Repository;
using Tasty_Treat_be.Models;

namespace Tasty_Treat_be.Repositories
{
    public class CustomizationOptionRepository : Repository<CustomizationOption>, ICustomizationOptionRepository
    {
        public CustomizationOptionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<CustomizationOption>> GetByItemIdAsync(int itemId)
        {
            return await _dbSet.Where(c => c.ItemId == itemId).ToListAsync();
        }
    }
}
