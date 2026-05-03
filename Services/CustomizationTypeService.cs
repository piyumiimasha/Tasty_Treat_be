using AutoMapper;
using Tasty_Treat_be.DTOs;
using Tasty_Treat_be.Interfaces.Repository;
using Tasty_Treat_be.Interfaces.Service;
using Tasty_Treat_be.Models;

namespace Tasty_Treat_be.Services
{
    public class CustomizationTypeService : ICustomizationTypeService
    {
        private readonly ICustomizationTypeRepository _repo;
        private readonly IMapper _mapper;

        public CustomizationTypeService(ICustomizationTypeRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CustomizationTypeDto>> GetAllAsync()
        {
            var types = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<CustomizationTypeDto>>(types);
        }

        public async Task<CustomizationTypeDto?> GetByIdAsync(int id)
        {
            var type = await _repo.GetByIdAsync(id);
            return type != null ? _mapper.Map<CustomizationTypeDto>(type) : null;
        }

        public async Task<CustomizationTypeDto> CreateAsync(CreateCustomizationTypeDto dto)
        {
            var type = _mapper.Map<CustomizationType>(dto);
            var created = await _repo.AddAsync(type);
            return _mapper.Map<CustomizationTypeDto>(created);
        }

        public async Task<CustomizationTypeDto> UpdateAsync(int id, UpdateCustomizationTypeDto dto)
        {
            var type = await _repo.GetByIdAsync(id);
            if (type == null)
                throw new KeyNotFoundException($"CustomizationType with id {id} not found");

            if (!string.IsNullOrEmpty(dto.Name))
                type.Name = dto.Name;

            var updated = await _repo.UpdateAsync(type);
            return _mapper.Map<CustomizationTypeDto>(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }
    }
}
