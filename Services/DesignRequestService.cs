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
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly ILogger<DesignRequestService> _logger;

        public DesignRequestService(IDesignRequestRepository repo, IOrderService orderService, IMapper mapper, ILogger<DesignRequestService> logger)
        {
            _repo = repo;
            _orderService = orderService;
            _mapper = mapper;
            _logger = logger;
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
                CustomerId = dto.CustomerId,
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

        public async Task<DesignRequestDto> UpdateStatusAsync(int id, string status, decimal? quotedPrice = null)
        {
            var item = await _repo.GetByIdAsync(id);
            if (item == null) throw new KeyNotFoundException($"DesignRequest with id {id} not found");
            item.Status = status;
            if (quotedPrice.HasValue) item.QuotedPrice = quotedPrice;
            var updated = await _repo.UpdateAsync(item);

            if (status.Equals("approved", StringComparison.OrdinalIgnoreCase))
            {
                if (!item.CustomerId.HasValue)
                {
                    _logger.LogWarning("DesignRequest {Id} approved but has no CustomerId — order not created.", id);
                }
                else
                {
                    try
                    {
                        var order = await _orderService.CreateAsync(new CreateOrderDto
                        {
                            CustomerId = item.CustomerId.Value,
                            Status = "Pending",
                            SpecialInstructions = item.Message,
                            TotalAmount = item.QuotedPrice ?? 0,
                        });
                        _logger.LogInformation("Order {OrderId} created from DesignRequest {RequestId}.", order.OrderId, id);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to create order from DesignRequest {Id}.", id);
                        throw;
                    }
                }
            }

            return _mapper.Map<DesignRequestDto>(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }
    }
}
