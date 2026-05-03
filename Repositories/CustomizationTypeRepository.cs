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
    }
}
