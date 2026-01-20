using Tasty_Treat_be.Models;

namespace Tasty_Treat_be.Interfaces.Repository
{
    public interface IItemRepository : IRepository<Item>
    {
        Task<IEnumerable<Item>> GetByCategoryAsync(string category);
    }
}
