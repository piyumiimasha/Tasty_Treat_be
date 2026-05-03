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

        public override async Task<CustomizationOption?> GetByIdAsync(int id)
        {
            return await _context.CustomizationOptions
                .Include(o => o.CustomizationType)
                .FirstOrDefaultAsync(o => o.OptionId == id);
        }

        public override async Task<IEnumerable<CustomizationOption>> GetAllAsync()
        {
            return await _context.CustomizationOptions
                .Include(o => o.CustomizationType)
                .ToListAsync();
        }
    }
}
