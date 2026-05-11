using Tasty_Treat_be.Models;

namespace Tasty_Treat_be.Interfaces.Repository
{
    public interface IItemFlavourRepository : IRepository<ItemFlavour>
    {
        Task<IEnumerable<ItemFlavour>> GetByItemIdAsync(int itemId);
        Task<IEnumerable<string>> GetDistinctNamesAsync();
    }
}
