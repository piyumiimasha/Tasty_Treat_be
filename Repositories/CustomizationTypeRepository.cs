using Microsoft.EntityFrameworkCore;
using Tasty_Treat_be.Data;
using Tasty_Treat_be.Interfaces.Repository;
using Tasty_Treat_be.Models;

namespace Tasty_Treat_be.Repositories
{
    public class CustomizationTypeRepository : Repository<CustomizationType>, ICustomizationTypeRepository
    {
        public CustomizationTypeRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            var type = await _context.CustomizationTypes
                .Include(t => t.CustomizationOptions)
                .FirstOrDefaultAsync(t => t.TypeId == id);

            if (type == null)
                return false;

            if (type.CustomizationOptions.Any())
            {
                _context.CustomizationOptions.RemoveRange(type.CustomizationOptions);
            }

            _context.CustomizationTypes.Remove(type);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
