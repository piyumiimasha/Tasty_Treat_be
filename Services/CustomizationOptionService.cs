using AutoMapper;
using Tasty_Treat_be.DTOs;
using Tasty_Treat_be.Interfaces.Repository;
using Tasty_Treat_be.Interfaces.Service;
using Tasty_Treat_be.Models;

namespace Tasty_Treat_be.Services
{
    public class CustomizationOptionService : ICustomizationOptionService
    {
        private readonly ICustomizationOptionRepository _customizationOptionRepository;
        private readonly IMapper _mapper;

        public CustomizationOptionService(ICustomizationOptionRepository customizationOptionRepository, IMapper mapper)
        {
            _customizationOptionRepository = customizationOptionRepository;
            _mapper = mapper;
        }

        public async Task<CustomizationOptionDto?> GetByIdAsync(int id)
        {
            var option = await _customizationOptionRepository.GetByIdAsync(id);
            return option != null ? _mapper.Map<CustomizationOptionDto>(option) : null;
        }

        public async Task<IEnumerable<CustomizationOptionDto>> GetAllAsync()
        {
            var options = await _customizationOptionRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CustomizationOptionDto>>(options);
        }

        public async Task<IEnumerable<CustomizationOptionDto>> GetByItemIdAsync(int itemId)
        {
            var options = await _customizationOptionRepository.GetByItemIdAsync(itemId);
            return _mapper.Map<IEnumerable<CustomizationOptionDto>>(options);
        }

        public async Task<CustomizationOptionDto> CreateAsync(CreateCustomizationOptionDto createCustomizationOptionDto)
        {
            var option = _mapper.Map<CustomizationOption>(createCustomizationOptionDto);
            var createdOption = await _customizationOptionRepository.AddAsync(option);
            return _mapper.Map<CustomizationOptionDto>(createdOption);
        }

        public async Task<CustomizationOptionDto> UpdateAsync(int id, UpdateCustomizationOptionDto updateCustomizationOptionDto)
        {
            var option = await _customizationOptionRepository.GetByIdAsync(id);
            if (option == null)
                throw new KeyNotFoundException($"CustomizationOption with id {id} not found");

            if (updateCustomizationOptionDto.ItemId.HasValue)
                option.ItemId = updateCustomizationOptionDto.ItemId.Value;
            if (!string.IsNullOrEmpty(updateCustomizationOptionDto.Name))
                option.Name = updateCustomizationOptionDto.Name;
            if (!string.IsNullOrEmpty(updateCustomizationOptionDto.Type))
                option.Type = updateCustomizationOptionDto.Type;
            if (updateCustomizationOptionDto.AdditionalPrice.HasValue)
                option.AdditionalPrice = updateCustomizationOptionDto.AdditionalPrice.Value;

            option.UpdatedAt = DateTime.UtcNow;

            var updatedOption = await _customizationOptionRepository.UpdateAsync(option);
            return _mapper.Map<CustomizationOptionDto>(updatedOption);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _customizationOptionRepository.DeleteAsync(id);
        }
    }
}
