using Tasty_Treat_be.DTOs;

namespace Tasty_Treat_be.Interfaces.Service
{
    public interface ICustomizationTypeService
    {
        Task<IEnumerable<CustomizationTypeDto>> GetAllAsync();
        Task<CustomizationTypeDto?> GetByIdAsync(int id);
        Task<CustomizationTypeDto> CreateAsync(CreateCustomizationTypeDto dto);
        Task<CustomizationTypeDto> UpdateAsync(int id, UpdateCustomizationTypeDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
