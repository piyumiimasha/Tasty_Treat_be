using Tasty_Treat_be.Models;

namespace Tasty_Treat_be.Interfaces.Repository
{
    public interface IInstantQuoteRepository : IRepository<InstantQuote>
    {
        Task<IEnumerable<InstantQuote>> GetByCustomerIdAsync(int customerId);
    }
}
