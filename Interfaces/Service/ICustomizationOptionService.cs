using Tasty_Treat_be.DTOs;

namespace Tasty_Treat_be.Interfaces.Service
{
    public interface ICustomizationOptionService
    {
        Task<CustomizationOptionDto?> GetByIdAsync(int id);
        Task<IEnumerable<CustomizationOptionDto>> GetAllAsync();
        Task<CustomizationOptionDto> CreateAsync(CreateCustomizationOptionDto createCustomizationOptionDto);
        Task<CustomizationOptionDto> UpdateAsync(int id, UpdateCustomizationOptionDto updateCustomizationOptionDto);
        Task<bool> DeleteAsync(int id);
    }
}
