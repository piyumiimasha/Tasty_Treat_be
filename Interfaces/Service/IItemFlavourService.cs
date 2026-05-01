using Tasty_Treat_be.DTOs;

namespace Tasty_Treat_be.Interfaces.Service
{
    public interface IItemFlavourService
    {
        Task<IEnumerable<ItemFlavourDto>> GetByItemIdAsync(int itemId);
        Task<ItemFlavourDto?> GetByIdAsync(int id);
        Task<ItemFlavourDto> CreateAsync(int itemId, CreateItemFlavourDto dto);
        Task<ItemFlavourDto> UpdateAsync(int id, UpdateItemFlavourDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
