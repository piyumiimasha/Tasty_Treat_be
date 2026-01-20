using Tasty_Treat_be.DTOs;

namespace Tasty_Treat_be.Interfaces.Service
{
    public interface IOrderItemService
    {
        Task<OrderItemDto?> GetByIdAsync(int id);
        Task<IEnumerable<OrderItemDto>> GetAllAsync();
        Task<IEnumerable<OrderItemDto>> GetByOrderIdAsync(int orderId);
        Task<OrderItemDto> CreateAsync(CreateOrderItemDto createOrderItemDto);
        Task<OrderItemDto> UpdateAsync(int id, UpdateOrderItemDto updateOrderItemDto);
        Task<bool> DeleteAsync(int id);
    }
}
