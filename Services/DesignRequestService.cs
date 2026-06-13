using System.Text.Json.Nodes;
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
        private readonly IInstantQuoteService _quoteService;
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;
        private readonly ILogger<DesignRequestService> _logger;

        public DesignRequestService(
            IDesignRequestRepository repo,
            IInstantQuoteService quoteService,
            INotificationService notificationService,
            IMapper mapper,
            ILogger<DesignRequestService> logger)
        {
            _repo = repo;
            _quoteService = quoteService;
            _notificationService = notificationService;
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

            await _notificationService.NotifyRoleAsync(
                "Admin",
                "DesignRequest",
                $"New custom design request from {dto.CustomerName}.",
                created.DesignRequestId);

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

        public async Task<DesignRequestDto> UpdateStatusAsync(int id, string status, decimal? quotedPrice = null, string? adminMessage = null)
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
                    _logger.LogWarning("DesignRequest {Id} approved but CustomerId is null — cannot add to cart.", id);
                }
                else
                {
                    _logger.LogInformation("Adding DesignRequest {Id} to cart for customer {CustomerId} at price {Price}.", id, item.CustomerId.Value, item.QuotedPrice);
                    await AddToCartAsync(item.CustomerId.Value, id, item.QuotedPrice ?? 0, item.ImageUrl, item.Message);
                    await _notificationService.NotifyUserAsync(
                        item.CustomerId.Value,
                        "OrderStatus",
                        $"Your custom cake design has been priced at Rs. {item.QuotedPrice:N2}. You can now proceed to checkout!",
                        id);
                }
            }
            else if (status.Equals("declined", StringComparison.OrdinalIgnoreCase) || status.Equals("cancelled", StringComparison.OrdinalIgnoreCase))
            {
                if (item.CustomerId.HasValue)
                {
                    var declineMessage = string.IsNullOrWhiteSpace(adminMessage)
                        ? "Your custom cake design request has been declined."
                        : $"Your custom cake design request has been declined as: {adminMessage}";

                    await _notificationService.NotifyUserAsync(
                        item.CustomerId.Value,
                        "OrderStatus",
                        declineMessage,
                        id);
                }
            }

            return _mapper.Map<DesignRequestDto>(updated);
        }

        private async Task AddToCartAsync(int customerId, int designRequestId, decimal price, string? imageUrl, string? message)
        {
            var quotes = await _quoteService.GetByCustomerIdAsync(customerId);
            var activeQuote = quotes.FirstOrDefault(q => q.ConvertedOrderId == null);

            var newItem = new JsonObject
            {
                ["cartItemId"] = $"custom-{designRequestId}",
                ["itemId"] = 0,
                ["name"] = $"Custom Cake Design #{designRequestId}",
                ["image"] = imageUrl ?? "",
                ["price"] = price,
                ["quantity"] = 1,
                ["size"] = "",
                ["flavor"] = message ?? "",
                ["isCustom"] = true,
                ["designRequestId"] = designRequestId,
                ["isPendingApproval"] = false,
            };

            if (activeQuote == null)
            {
                var items = new JsonArray { newItem };
                decimal tax = Math.Round(price * 0.1m, 2);
                await _quoteService.CreateAsync(new CreateInstantQuoteDto
                {
                    CustomerId = customerId,
                    Items = items.ToJsonString(),
                    Tax = tax,
                    Discount = 0,
                    DeliveryFee = 0,
                    EstimatedPrice = Math.Round(price + tax, 2),
                });
            }
            else
            {
                JsonArray items;
                try
                {
                    items = JsonNode.Parse(activeQuote.Items)?.AsArray() ?? new JsonArray();
                }
                catch
                {
                    items = new JsonArray();
                }
                items.Add(newItem);
                decimal subtotal = items
                    .OfType<JsonObject>()
                    .Sum(o => (o["price"]?.GetValue<decimal>() ?? 0) * (o["quantity"]?.GetValue<int>() ?? 1));
                decimal tax = Math.Round(subtotal * 0.1m, 2);
                await _quoteService.UpdateAsync(activeQuote.QuoteId, new UpdateInstantQuoteDto
                {
                    Items = items.ToJsonString(),
                    Tax = tax,
                    EstimatedPrice = Math.Round(subtotal + tax, 2),
                });
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }
    }
}
