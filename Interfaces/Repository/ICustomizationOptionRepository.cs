using Tasty_Treat_be.Models;

namespace Tasty_Treat_be.Interfaces.Repository
{
    public interface ICustomizationOptionRepository : IRepository<CustomizationOption>
    {
        Task<IEnumerable<CustomizationOption>> GetByItemIdAsync(int itemId);
    }
}
