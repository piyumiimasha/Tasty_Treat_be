using Tasty_Treat_be.Models;

namespace Tasty_Treat_be.Interfaces.Repository
{
    public interface IReviewRepository : IRepository<Review>
    {
        Task<IEnumerable<Review>> GetByItemIdAsync(int itemId);
        Task<IEnumerable<Review>> GetByCustomerIdAsync(int customerId);
    }
}
