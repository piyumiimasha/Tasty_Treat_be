using Microsoft.EntityFrameworkCore;
using Tasty_Treat_be.Data;
using Tasty_Treat_be.Interfaces.Repository;
using Tasty_Treat_be.Models;

namespace Tasty_Treat_be.Repositories
{
    public class InstantQuoteRepository : Repository<InstantQuote>, IInstantQuoteRepository
    {
        public InstantQuoteRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<InstantQuote>> GetByCustomerIdAsync(int customerId)
        {
            return await _dbSet.Where(q => q.CustomerId == customerId).ToListAsync();
        }
    }
}
