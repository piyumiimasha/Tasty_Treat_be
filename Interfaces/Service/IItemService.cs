using Tasty_Treat_be.DTOs;

namespace Tasty_Treat_be.Interfaces.Service
{
    public interface IItemService
    {
        Task<ItemDto?> GetByIdAsync(int id);
        Task<IEnumerable<ItemDto>> GetAllAsync();
        Task<IEnumerable<ItemDto>> GetByCategoryAsync(string category);
        Task<ItemDto> CreateAsync(CreateItemDto createItemDto);
        Task<ItemDto> UpdateAsync(int id, UpdateItemDto updateItemDto);
        Task<bool> DeleteAsync(int id);
    }
}
