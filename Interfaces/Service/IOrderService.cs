using Tasty_Treat_be.DTOs;

namespace Tasty_Treat_be.Interfaces.Service
{
    public interface IOrderService
    {
        Task<OrderDto?> GetByIdAsync(int id);
        Task<IEnumerable<OrderDto>> GetAllAsync();
        Task<IEnumerable<OrderDto>> GetByCustomerIdAsync(int customerId);
        Task<IEnumerable<OrderDto>> GetByStatusAsync(string status);
        Task<OrderDto> CreateAsync(CreateOrderDto createOrderDto);
        Task<OrderDto> UpdateAsync(int id, UpdateOrderDto updateOrderDto);
        Task<bool> DeleteAsync(int id);
    }
}
