using AutoMapper;
using Tasty_Treat_be.DTOs;
using Tasty_Treat_be.Interfaces.Repository;
using Tasty_Treat_be.Interfaces.Service;
using Tasty_Treat_be.Models;

namespace Tasty_Treat_be.Services
{
    public class DesignRequestService : IDesignRequestService
    {
        private readonly IDesignRequestRepository _repo;
        private readonly IMapper _mapper;

        public DesignRequestService(IDesignRequestRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DesignRequestDto>> GetAllAsync()
        {
            var items = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<DesignRequestDto>>(items);
        }

        public async Task<DesignRequestDto?> GetByIdAsync(int id)
        {
            var item = await _repo.GetByIdAsync(id);
            return item != null ? _mapper.Map<DesignRequestDto>(item) : null;
        }

        public async Task<DesignRequestDto> CreateAsync(CreateDesignRequestDto dto)
        {
            var request = new DesignRequest
            {
                CustomerName = dto.CustomerName,
                Message = dto.Message,
                Status = "pending",
                CreatedAt = DateTime.UtcNow,
            };
            var created = await _repo.AddAsync(request);
            return _mapper.Map<DesignRequestDto>(created);
        }

        public async Task<DesignRequestDto> UpdateImageAsync(int id, string? imageUrl)
        {
            var item = await _repo.GetByIdAsync(id);
            if (item == null) throw new KeyNotFoundException($"DesignRequest with id {id} not found");
            item.ImageUrl = imageUrl;
            var updated = await _repo.UpdateAsync(item);
            return _mapper.Map<DesignRequestDto>(updated);
        }

        public async Task<DesignRequestDto> UpdateStatusAsync(int id, string status)
        {
            var item = await _repo.GetByIdAsync(id);
            if (item == null) throw new KeyNotFoundException($"DesignRequest with id {id} not found");
            item.Status = status;
            var updated = await _repo.UpdateAsync(item);
            return _mapper.Map<DesignRequestDto>(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }
    }
}
