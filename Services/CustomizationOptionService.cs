using AutoMapper;
using Tasty_Treat_be.DTOs;
using Tasty_Treat_be.Interfaces.Repository;
using Tasty_Treat_be.Interfaces.Service;
using Tasty_Treat_be.Models;

namespace Tasty_Treat_be.Services
{
    public class CustomizationOptionService : ICustomizationOptionService
    {
        private readonly ICustomizationOptionRepository _repo;
        private readonly IMapper _mapper;

        public CustomizationOptionService(ICustomizationOptionRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<CustomizationOptionDto?> GetByIdAsync(int id)
        {
            var option = await _repo.GetByIdAsync(id);
            return option != null ? _mapper.Map<CustomizationOptionDto>(option) : null;
        }

        public async Task<IEnumerable<CustomizationOptionDto>> GetAllAsync()
        {
            var options = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<CustomizationOptionDto>>(options);
        }

        public async Task<CustomizationOptionDto> CreateAsync(CreateCustomizationOptionDto dto)
        {
            var option = _mapper.Map<CustomizationOption>(dto);
            var created = await _repo.AddAsync(option);
            // Reload with type navigation to populate TypeName/TypeDisplayName
            var withType = await _repo.GetByIdAsync(created.OptionId);
            return _mapper.Map<CustomizationOptionDto>(withType!);
        }

        public async Task<CustomizationOptionDto> UpdateAsync(int id, UpdateCustomizationOptionDto dto)
        {
            var option = await _repo.GetByIdAsync(id);
            if (option == null)
                throw new KeyNotFoundException($"CustomizationOption with id {id} not found");

            if (!string.IsNullOrEmpty(dto.Name))
                option.Name = dto.Name;
            if (dto.TypeId.HasValue)
                option.TypeId = dto.TypeId.Value;
            if (dto.AdditionalPrice.HasValue)
                option.AdditionalPrice = dto.AdditionalPrice.Value;

            option.UpdatedAt = DateTime.UtcNow;

            var updated = await _repo.UpdateAsync(option);
            var withType = await _repo.GetByIdAsync(updated.OptionId);
            return _mapper.Map<CustomizationOptionDto>(withType!);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }
    }
}
