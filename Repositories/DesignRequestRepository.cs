using Tasty_Treat_be.Data;
using Tasty_Treat_be.Interfaces.Repository;
using Tasty_Treat_be.Models;

namespace Tasty_Treat_be.Repositories
{
    public class DesignRequestRepository : Repository<DesignRequest>, IDesignRequestRepository
    {
        public DesignRequestRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
